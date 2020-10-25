using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterInfo_Old : MonoBehaviour
{
    // Base Stats
    public int strength;
    public int constitution;

    // Current Stats
    public int currentHealth;

    // Animator
    Animator anim;

    // UI
    Transform healthbar;
    float maxHealthbar;

    void Start()
    {
        currentHealth = constitution;

        anim = GetComponent<Animator>();

        healthbar = transform.GetChild(0);
        maxHealthbar = healthbar.localScale.x;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        float healthRemaining = healthbar.localScale.x - (maxHealthbar * (damage * 1f / constitution)); // Gotta do the 1f for float calc
        healthbar.localScale = new Vector2(healthRemaining, healthbar.localScale.y);
        if (currentHealth <= 0)
        {
            StartCoroutine(WaitDeathAnimation(0.2f));
        }
    }

    IEnumerator WaitDeathAnimation(float length)
    {
        // Deactivate AI Script and Vision Radius
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(length);

        anim.SetTrigger("Death");

        // if player
        if (gameObject.CompareTag("Player"))
        {
            GetComponent<PlayerController>().enabled = false;
        }

        // if enemy
        else if (gameObject.CompareTag("Enemy"))
        {
            // New collider for dead body
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = false;
            box.offset = new Vector2(0.01f, 0.06f);
            box.size = new Vector2(0.38f, 0.08f);
            box.enabled = true;
        }

        gameObject.tag = "Corpse";
    }
}
