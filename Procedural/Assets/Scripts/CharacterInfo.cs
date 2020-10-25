using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    // Base Stats
    public int strength;
    public int constitution;

    // Current Stats
    public int currentHealth;

    // Character Componenets
    SpriteRenderer sprite;
    Animator anim;

    // UI
    Transform healthbar;
    float maxHealthbar;

    void Start()
    {
        currentHealth = constitution;

        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        healthbar = transform.Find("HealthbarUI").Find("Healthbar");
        maxHealthbar = healthbar.localScale.x;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(HurtAnimation());
        float healthRemaining = healthbar.localScale.x - (maxHealthbar * (damage * 1f / constitution)); // Gotta do the 1f for float calc
        healthbar.localScale = new Vector2(healthRemaining, healthbar.localScale.y);
        if (currentHealth <= 0)
        {
            StartCoroutine(WaitDeathAnimation(0.2f));
        }
    }

    IEnumerator HurtAnimation()
    {
        sprite.color = new Color(1, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        sprite.color = new Color(1, 1, 1);
    }

    IEnumerator WaitDeathAnimation(float length)
    {
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
            GetComponent<EnemyAI>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<CharacterInfo>().enabled = false;
        }

        gameObject.tag = "Corpse";
    }
}
