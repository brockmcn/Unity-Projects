using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public int walkSpeed;
    public int sprintSpeed;
    private int currentSpeed;

    public Vector2[] patrolPoints;
    private Vector2 nextPatrolPoint;
    private int patrolPointNum;

    private bool isMoving;

    private bool rotateDown;
    private bool rotateUp;

    private int randomMovement;

    public GameObject gridObject;
    private Grid grid;

    public bool playerFound;
    public GameObject player;
    public Vector2 playerDirection;
    Vector2 nextToPos;

    float combatTimer;
    float time;

    void Start()
    {
        currentSpeed = walkSpeed;

        if (patrolPoints.Length > 0)
            nextPatrolPoint = patrolPoints[patrolPointNum];
        else
            nextPatrolPoint = transform.position;

        randomMovement = Random.Range(0, 2); // Horizontal movement first is 1, Vertical is 2

        grid = gridObject.GetComponent<Grid>();

        nextToPos = new Vector2((int)player.transform.position.x + 0.5f, (int)player.transform.position.y + 1.5f);

        combatTimer = 0;
        time = Time.deltaTime;
    }

    void Update()
    {
        Movement();

        if (playerFound)
        {
            currentSpeed = sprintSpeed;
            GoToPlayer();
        }

        Combat();
    }

    void Movement()
    {
        if (randomMovement == 0 || transform.position.y == nextPatrolPoint.y)
        {
            grid.SetTileTaken(transform.position.x, transform.position.y, false, gameObject);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPatrolPoint.x, transform.position.y), currentSpeed * Time.deltaTime);
            rotateUp = true;
        }

        if (randomMovement == 1 || transform.position.x == nextPatrolPoint.x)
        {
            grid.SetTileTaken(transform.position.x, transform.position.y, false, gameObject);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, nextPatrolPoint.y), currentSpeed * Time.deltaTime);
            rotateDown = true;
        }

        Rotation();

        if (patrolPoints.Length > 0)
            Patrol();
    }

    void Rotation()
    {
        if (randomMovement == 0 || rotateUp)
        {
            if (nextPatrolPoint.x < transform.position.x) transform.eulerAngles = new Vector3(0, 0, 90);
            if (nextPatrolPoint.x > transform.position.x) transform.eulerAngles = new Vector3(0, 0, -90);
            rotateUp = false;
        }

        if (randomMovement == 1 || rotateDown)
        {
            if (nextPatrolPoint.y < transform.position.y) transform.eulerAngles = new Vector3(0, 0, 180);
            if (nextPatrolPoint.y > transform.position.y) transform.eulerAngles = new Vector3(0, 0, 0);
            rotateDown = false;
        }
    }

    void Patrol()
    {
        if ((Vector2)transform.position == nextPatrolPoint && !isMoving)
        {
            isMoving = true;
            if (patrolPoints.Length == patrolPointNum + 1) patrolPointNum = 0;
            else patrolPointNum++;
            StartCoroutine(PatrolWait());
        }
    }

    IEnumerator PatrolWait()
    {
        yield return new WaitForSeconds(2);
        nextPatrolPoint = patrolPoints[patrolPointNum];
        isMoving = false;
    }

    void GoToPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > Mathf.Abs(transform.position.y - player.transform.position.y))
        {
            if (transform.position.x > player.transform.position.x)
                nextToPos = new Vector2((int)player.transform.position.x + 1.5f, (int)player.transform.position.y + 0.5f);
            else if (transform.position.x < player.transform.position.x)
                nextToPos = new Vector2((int)player.transform.position.x - 0.5f, (int)player.transform.position.y + 0.5f);
        }

        else if (Mathf.Abs(transform.position.x - player.transform.position.x) < Mathf.Abs(transform.position.y - player.transform.position.y))
            {
            if (transform.position.y > player.transform.position.y)
                nextToPos = new Vector2((int)player.transform.position.x + 0.5f, (int)player.transform.position.y + 1.5f);
            else if (transform.position.y < player.transform.position.y)
                nextToPos = new Vector2((int)player.transform.position.x + 0.5f, (int)player.transform.position.y - 0.5f);
        }

        nextPatrolPoint = nextToPos;

        // Rotate towards player
        if (transform.position.x == player.transform.position.x && Vector2.Distance(transform.position, player.transform.position) == 1)
            Rotator.RotateTowards(gameObject, player.transform.position);
        if (transform.position.y == player.transform.position.y && Vector2.Distance(transform.position, player.transform.position) == 1)
            Rotator.RotateTowards(gameObject, player.transform.position);
    }

    void Combat()
    {
        int attackSpeed = 4;

        if (Vector3.Distance(transform.position, player.transform.position) <= 1
            && Rotator.IsRotatedTowards(gameObject, player.transform.position))
        {
            combatTimer += time;

            if (combatTimer >= attackSpeed)
            {
                player.GetComponentInChildren<Health>().Damage(10);
                combatTimer = 0;
            }
        }
    }
}
