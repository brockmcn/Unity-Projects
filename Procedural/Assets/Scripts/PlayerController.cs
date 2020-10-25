using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Components
    Rigidbody2D body;
    Animator anim;
    Camera playerCamera;

    // Movement Variables
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    public float speed;

    // Indicator Variables
    GameObject indicator;
    public Transform center;
    private Vector3 v;
    public Texture2D cursor;

    // Combat Variables
    bool inCombat;
    bool canAttack;
    bool isAttacking;
    Collider2D hitbox;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCamera = transform.GetComponentInChildren<Camera>();

        indicator = transform.Find("Indicator").gameObject;
        v = (indicator.transform.position - center.position);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

        hitbox = indicator.GetComponentInChildren<PolygonCollider2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (!inCombat)
        {
            if (indicator.activeSelf)
                indicator.SetActive(false);

            // Idle
            if (horizontal != 0 || vertical != 0)
            {
                if (horizontal < 0)
                    anim.SetInteger("AnimState", 2);
                else if (horizontal > 0)
                    anim.SetInteger("AnimState", 3);
                else if (vertical < 0)
                    anim.SetInteger("AnimState", 1);
                else if (vertical > 0)
                    anim.SetInteger("AnimState", 0);
            }

            // Movement
            if (Input.GetKeyUp("w"))
                anim.SetInteger("AnimState", 4);
            else if (Input.GetKeyUp("a"))
                anim.SetInteger("AnimState", 6);
            else if (Input.GetKeyUp("s"))
                anim.SetInteger("AnimState", 5);
            else if (Input.GetKeyUp("d"))
                anim.SetInteger("AnimState", 7);
        }

        else if (inCombat)
        {
            FollowCursor();

            // Attacking
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                isAttacking = true;
                hitbox.transform.localScale = Vector2.one;

                if (indicator.transform.localEulerAngles.z < 45 || indicator.transform.localEulerAngles.z > 315)
                    anim.SetTrigger("AttackUp");
                else if (indicator.transform.localEulerAngles.z > 45 && indicator.transform.localEulerAngles.z < 135)
                    anim.SetTrigger("AttackLeft");
                else if (indicator.transform.localEulerAngles.z > 135 && indicator.transform.localEulerAngles.z < 225)
                    anim.SetTrigger("AttackDown");
                else if (indicator.transform.localEulerAngles.z > 225 && indicator.transform.localEulerAngles.z < 315)
                    anim.SetTrigger("AttackRight");
            }

            // One attack at a time please
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("AttackDown") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("AttackLeft") || anim.GetCurrentAnimatorStateInfo(0).IsName("AttackRight"))
            {
                canAttack = false;
                hitbox.transform.parent = transform;
            }

            else if (!canAttack)
            {
                isAttacking = false;
                hitbox.transform.localScale = Vector2.zero;
                hitbox.transform.parent = indicator.transform;
                hitbox.transform.localPosition = Vector2.zero;
                hitbox.transform.rotation = new Quaternion(0, 0, 0, 0);
                canAttack = true;
            }
        }
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

    void FollowCursor()
    {
        if (!indicator.activeSelf)
            indicator.SetActive(true);

        // Indicator rotation
        Vector3 centerScreenPos = playerCamera.WorldToScreenPoint(center.position);
        Vector3 dir = Input.mousePosition - centerScreenPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        indicator.transform.position = center.position + q * v;
        indicator.transform.rotation = q;

        // Idle states in combat
        if (horizontal != 0 || vertical != 0)
        {
            if (indicator.transform.localEulerAngles.z < 45 || indicator.transform.localEulerAngles.z > 315)
                anim.SetInteger("AnimState", 0);
            else if (indicator.transform.localEulerAngles.z > 45 && indicator.transform.localEulerAngles.z < 135)
                anim.SetInteger("AnimState", 2);
            else if (indicator.transform.localEulerAngles.z > 135 && indicator.transform.localEulerAngles.z < 225)
                anim.SetInteger("AnimState", 1);
            else if (indicator.transform.localEulerAngles.z > 225 && indicator.transform.localEulerAngles.z < 315)
                anim.SetInteger("AnimState", 3);
        }

        // Moving in combat
        else
        {
            if (indicator.transform.localEulerAngles.z < 45 || indicator.transform.localEulerAngles.z > 315)
                anim.SetInteger("AnimState", 4);
            else if (indicator.transform.localEulerAngles.z > 45 && indicator.transform.localEulerAngles.z < 135)
                anim.SetInteger("AnimState", 6);
            else if (indicator.transform.localEulerAngles.z > 135 && indicator.transform.localEulerAngles.z < 225)
                anim.SetInteger("AnimState", 5);
            else if (indicator.transform.localEulerAngles.z > 225 && indicator.transform.localEulerAngles.z < 315)
                anim.SetInteger("AnimState", 7);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Enter combat if enemy finds you
        if (collider.CompareTag("Enemy") && !inCombat)
            inCombat = true;

        // If sword hits enemy
        if (collider.CompareTag("Enemy") && isAttacking)
        {
            collider.GetComponent<CharacterInfo>().Damage(GetComponent<CharacterInfo>().strength);
            isAttacking = false;
        }

        // Enter new room
        if (collider.CompareTag("Door"))
        {
            string checkStr = "";
            string doorStr = "";

            if (collider.gameObject.name.Equals("Door (U)"))
            {
                checkStr = "(U)";
                doorStr = "(D)";
            }
            else if (collider.gameObject.name.Equals("Door (D)"))
            {
                checkStr = "(D)";
                doorStr = "(U)";
            }
            else if (collider.gameObject.name.Equals("Door (L)"))
            {
                checkStr = "(L)";
                doorStr = "(R)";
            }
            else if (collider.gameObject.name.Equals("Door (R)"))
            {
                checkStr = "(R)";
                doorStr = "(L)";
            }

            GameObject nextRoom = collider.transform.parent.parent.Find("Checkers").Find("Checker " + checkStr).GetComponent<Checker>().nextRoom;
            Vector2 doorPos = nextRoom.transform.Find("Doors").Find("Door " + doorStr).position;
            transform.position = new Vector2(doorPos.x, doorPos.y);
            nextRoom.transform.Find("SpawnPoints").gameObject.SetActive(true);
        }
    }
}
