/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAI_Old : MonoBehaviour
{
    bool playerFound;
    bool inCombat;
    GameObject player;
    public int speed;
    Collider2D visionRadius;
    Animator anim;
    SpriteRenderer sprite;
    public bool isBlocking;

    void Start()
    {
        visionRadius = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFound && !isRooted())
        {
            isBlocking = false;
            if (Vector2.Distance(transform.position, player.transform.position) > 5)
            {
                playerFound = false;
                visionRadius.enabled = true;
            }

            // Flip Sprite
            if (transform.position.x < player.transform.position.x)
                sprite.flipX = true;
            else if (transform.position.x > player.transform.position.x)
                sprite.flipX = false;

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            anim.SetInteger("AnimState", 2);
        }
        else if (inCombat)
        {
            if (!isBlocking && !isRooted())
            {
                float x = Random.Range(0.5f, 2f);
                StartCoroutine(Combat(x));
            }
        }
        else
            anim.SetInteger("AnimState", 0);
    }

    bool isRooted()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Daze");
    }

    IEnumerator OnCompleteAttackAnimation(float length)
    {
        yield return new WaitForSeconds(length);

        if (player.GetComponent<PlayerController_Old>().isBlocking)
            anim.SetTrigger("Daze");

        else if (inCombat && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            player.GetComponent<CharacterInfo>().Damage(GetComponent<CharacterInfo>().strength);
        }
    }

    IEnumerator Combat(float length)
    {
        isBlocking = true;
        anim.SetInteger("AnimState", 1);
        yield return new WaitForSeconds(length);
        if (inCombat && !isRooted())
        {
            isBlocking = false;
            anim.SetTrigger("Attack");
            StartCoroutine(OnCompleteAttackAnimation(0.4f));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerFound = true;
            visionRadius.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerFound = false;
            inCombat = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerFound = false;
            inCombat = false;
            visionRadius.enabled = true;
        }
    }
}*/
