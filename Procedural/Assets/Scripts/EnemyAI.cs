using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float speed;
    bool playerFound;
    public int attackSpeed;
    bool oor;  // Out of range from the player
    GameObject player;
    Animator anim;
    bool canAttack;

    void Start()
    {
        canAttack = true;
        oor = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerFound)
        {
            if (oor)
            {
                if (Mathf.Abs(transform.position.y - player.transform.position.y) > Mathf.Abs(transform.position.x - player.transform.position.x))
                {
                    if (transform.position.y < player.transform.position.y)
                        anim.SetInteger("AnimState", 0);
                    else
                        anim.SetInteger("AnimState", 1);
                }
                else
                {
                    if (transform.position.x > player.transform.position.x)
                        anim.SetInteger("AnimState", 2);
                    else
                        anim.SetInteger("AnimState", 3);
                }

                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }

            else if (!oor)
            {
                if (canAttack)
                    StartCoroutine(Attack());
                else
                {
                    if (Mathf.Abs(transform.position.y - player.transform.position.y) > Mathf.Abs(transform.position.x - player.transform.position.x))
                    {
                        if (transform.position.y < player.transform.position.y)
                            anim.SetInteger("AnimState", 4);
                        else
                            anim.SetInteger("AnimState", 5);
                    }
                    else
                    {
                        if (transform.position.x > player.transform.position.x)
                            anim.SetInteger("AnimState", 6);
                        else
                            anim.SetInteger("AnimState", 7);
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;

        player.GetComponent<CharacterInfo>().Damage(GetComponent<CharacterInfo>().strength);

        if (Mathf.Abs(transform.position.y - player.transform.position.y) > Mathf.Abs(transform.position.x - player.transform.position.x))
        {
            if (transform.position.y < player.transform.position.y)
                anim.SetTrigger("AttackUp");
            else
                anim.SetTrigger("AttackDown");
        }
        else
        {
            if (transform.position.x > player.transform.position.x)
                anim.SetTrigger("AttackLeft");
            else
                anim.SetTrigger("AttackRight");
        }

        yield return new WaitForSeconds(1/attackSpeed + 1);

        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            player = collider.gameObject;
            playerFound = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            player = collision.gameObject;
            oor = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            player = collision.gameObject;
            oor = true;
        }
    }
}
