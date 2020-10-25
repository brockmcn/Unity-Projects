/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Old : MonoBehaviour
{

    Rigidbody2D body;

    float horizontal;
    float vertical;
    float attack;
    bool inCombat;
    public bool isBlocking;
    GameObject enemy;
    float moveLimiter = 0.7f;
    Animator anim;
    SpriteRenderer sprite;

    public float speed = 5.0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        attack = Input.GetAxisRaw("Attack");

        // Flip Sprite
        if (horizontal > 0)
            sprite.flipX = true;
        else if (horizontal < 0)
            sprite.flipX = false;

        // Passive Animation States
        if (Input.GetKey("w") && !isRooted())
        {
            anim.SetInteger("AnimState", 1);
            isBlocking = true;
        }
        else if (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon)
            anim.SetInteger("AnimState", 2);
        else
            anim.SetInteger("AnimState", 0);

        // Attacking
        if (Input.GetKeyDown("q") && !isRooted())
        {
            anim.SetTrigger("Attack");
            StartCoroutine(OnCompleteAttackAnimation(0.4f));
        }

        if (isRooted() || anim.GetInteger("AnimState") == 1)
        {
            horizontal = 0;
            vertical = 0;
        }

        // Stop blocking
        if (Input.GetKeyUp("w"))
            isBlocking = false;
    }

    bool isRooted()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Daze");
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    IEnumerator OnCompleteAttackAnimation(float length)
    {
        yield return new WaitForSeconds(length);

        if (inCombat)
        {
            if (enemy.GetComponent<EnemyAI>().isBlocking)
                anim.SetTrigger("Daze");
            else
                enemy.GetComponent<CharacterInfo>().Damage(GetComponent<CharacterInfo>().strength);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            inCombat = true;
            enemy = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            inCombat = false;
            enemy = null;
        }
    }
}*/
