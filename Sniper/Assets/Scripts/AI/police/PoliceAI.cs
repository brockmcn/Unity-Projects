using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region oldai

//public class PoliceAI : MonoBehaviour
//{

//    //Get rid of all gameobject.finds in update function

//    public bool hostile;
//    private Vector3 playerPosition;
//    public bool ShotFired = false;

//    public bool isInCombat = false;

//    private GameObject player;

//    private NavMeshAgent agent;

//    public float shootingDelay = 1;

//    private bool isTakingCover = false;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//    }

//    void Update()
//    {
//        playerPosition = GameObject.Find("FPS Controller Rig Sniper Rifle").transform.position;
//        player = GameObject.Find("FPS Controller Rig Sniper Rifle");

//        if (ShotFired)
//        {
//            if (Vector3.Distance(playerPosition, transform.position) < 100)
//            {
//                hostile = true; //police heard it
//                HostileMode();
//            }
//            else
//            {
//                ShotFired = false; //police didnt hear it
//            }
//        }

//        if (Vector3.Distance(agent.transform.position, agent.destination) < 2 && hostile) //cant reach destination exactly, so just start firing
//        {
//            OpenFire();
//            agent.ResetPath();
//        }

//        if (player.GetComponent<PlayerHealth>().currentHealth <= 0 && hostile) //player is dead/was killed successfully
//        {
//            hostile = false;
//            //keep on walking
//        }
//    }

//    void Attack()
//    {
//        if (isInCombat)
//            return;

//        Debug.Log("going into attack mode");
//        agent.ResetPath();
//        if (Random.Range(0, 1) >= -5) //50% chance to take cover first
//        {
//            NavMeshHit hit;
//            if (agent.FindClosestEdge(out hit))
//            {
//                agent.SetDestination(hit.position);

//                isTakingCover = true;
//            }
//            else
//            {
//                Debug.Log("Couldnt find place to take cover at");
//            }
//        }
//        else
//        {
//            //open fire directly
//            OpenFire();
//        }
//    }

//    bool CanSeePlayer()
//    {
//        return GetComponentInChildren<Renderer>().isVisible;

//        RaycastHit hit;
//        if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), -Vector3.up, out hit))
//        {
//            if (hit.collider.name == "FPS Controller Rig Sniper Rifle")
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    void HostileMode()
//    {
//        if (Vector3.Distance(playerPosition, agent.transform.position) < 20 && CanSeePlayer())
//        {
//            agent.ResetPath();
//            Attack();
//        }

//        if (isInCombat)
//            return;

//        Debug.Log("going into hostile mode");
//        agent.speed *= 1.5f; //run instead of casually walking around
//        if (!CanSeePlayer())
//            if (Random.Range(0, 1) <= .2) //20% chance to not care because he didnt see it happen
//                return;

//        if (!isTakingCover)
//            agent.SetDestination(playerPosition); //move towards player player until he gets into shooting range
//    }

//    void OpenFire()
//    {
//        if (!isInCombat)
//        {
//            isInCombat = true;
//            transform.LookAt(player.transform);
//            StartCoroutine(ShootingThread());
//        }
//    }

//    IEnumerator ShootingThread()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(shootingDelay);

//            Debug.Log("Shot the player"); //shooting at player
//        }
//    }
//}

#endregion

public class PoliceAI : MonoBehaviour
{

    public float shootingDelay = 1f;
    public float hearingRange = 100f;
    public float shootingRange = 20f;
    public float fieldOfViewAngle = 90f;
    public float lookingRange = 100f;
    public float lookingInterval = 8f;
    public float fovAdjustment = 0f;

    private GameObject player;
    private NavMeshAgent agent;
    private bool shotFired;
    private Animator animator;
    private PoliceMovement movement;

    private PoliceShooting polShoot;

    private bool lookingForPlayer = false;
    private bool takingCover = false;
    private bool shooting = false;
    private bool movingIntoPosition = false;
    private bool movingToOtherPolice = false;
    
    private float lastTimeLookingAround;

    private bool foundPlayer = false;

    private void Start()
    {
        player = GameObject.Find("FPS Controller Rig Sniper Rifle");
        agent = GetComponentInParent<NavMeshAgent>();
        lastTimeLookingAround = Time.time;
        animator = GetComponent<Animator>();
        polShoot = GetComponentInChildren<PoliceShooting>();
        movement = GetComponent<PoliceMovement>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude / movement.maxSpeed);
        animator.SetBool("Shooting", shooting);

        Debug.DrawLine(agent.transform.position + Vector3.up, agent.destination + Vector3.up,Color.yellow);

        if (movingIntoPosition && Vector3.Distance(agent.transform.position, player.transform.position) < shootingRange) //now in range, look for the player
        {
            agent.ResetPath();
            ClearStates();
            LookForPlayer();
        }

        else if (takingCover && Vector3.Distance(agent.destination, agent.transform.position) < 2f) //took cover, now shoot
        {
            agent.ResetPath();
            ClearStates();
            agent.transform.LookAt(player.transform);
            ShootPlayer();
        }

        else if (movingToOtherPolice && Vector3.Distance(agent.transform.position, agent.destination) < 10f) //called to help, now in the area and looking for the player
        {
            agent.ResetPath();
            ClearStates();
            LookForPlayer();
        }

        else if (shooting && player.GetComponent<PlayerStats>().healthStat <= 0) //player is dead
        {
            ClearStates();
            StopCoroutine("Shooting");
            movement.Wandering = true;
        }

        else if (lookingForPlayer) //keep looking until he finds him
        {
            if (Time.time >= lastTimeLookingAround + lookingInterval)
            {
                LookForPlayer();
            }
        }

        if (lookingForPlayer && DoesViewContainPlayer()) //found the player while looking
        {
            foundPlayer = true;
            animator.SetTrigger("Found");
            Debug.Log("found player!!!!!!!!");
            ClearStates();
            if (Random.Range(0, 1) > 0f) //just for testing/change this later
            {
                TakeCover();
            }
            else
            {
                ShootPlayer();
            }
        }

        if (IsWandering() && DoesViewContainPlayer() && !GameObject.Find("Weapon Holder").GetComponent<SwitchWeapon>().noWeaponOut && takingCover == false) //player has weapon
        {
            //Debug.Log("Take Cover");
            movement.Wandering = false;
            TakeCover();
        }

    }

    private bool IsWandering()
    {
        return lookingForPlayer == false && takingCover == false && shooting == false && movingIntoPosition == false && movingToOtherPolice == false;
    }

    public bool DoesViewContainPlayer()
    {
        //Debug.Log("checking whether player is in the field of view");
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        //Debug.Log("calculated angles");

        if (angle <= fieldOfViewAngle / 2 + fovAdjustment)
        {
            //Debug.Log("in the angle");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction.normalized, out hit, lookingRange))
            {
                //Debug.Log("raycast hit " + hit.collider.gameObject);
                if (hit.collider.gameObject == player)
                {
                    //Debug.Log("not obstructed");
                    return true;
                }
            }
        }
        return false;
    }

    public void ShotFired()
    {
        if (IsWandering())
        {
            //Debug.Log("shot fired");
            shotFired = true;
            if (Vector3.Distance(player.transform.position, transform.position) <= hearingRange)
                Alerted();
            else
                shotFired = false;
        }
    }

    private void Alerted()
    {
        movement.Wandering = false;
        agent.speed = movement.maxSpeed;
        //Debug.Log("alerted");
        MoveToPlayer();
    }

    private void LookForPlayer()
    {
        StartCoroutine(LookForPlayerCoroutine());
        ClearStates();
        lookingForPlayer = true;
    }

    IEnumerator LookForPlayerCoroutine()
    {
        //Debug.Log("looking for player");
        agent.destination = new Vector3(Random.insideUnitCircle.x - .5f, 0, Random.insideUnitCircle.y - .5f) * 5 + agent.transform.position; //move randomly
        yield return new WaitForSeconds(2); //wait for movement to finish
        GetComponent<Animator>().SetTrigger("LookAround");
        lastTimeLookingAround = Time.time;
        fovAdjustment = 45f;
        yield return new WaitForSeconds(4);
        fovAdjustment = 0f;
    }

    private void MoveToPlayer()
    {
        //Debug.Log("moving to player");
        agent.destination = player.transform.position;
        ClearStates();
        movingIntoPosition = true;
    }

    private void ClearStates()
    {
        movingIntoPosition = false;
        takingCover = false;
        shooting = false;
        lookingForPlayer = false;
        movingToOtherPolice = false;
    }

    private void TakeCover()
    {
        //Debug.Log("taking cover");
        ClearStates();
        takingCover = true;

        NavMeshHit hit;

        if(agent.FindClosestEdge(out hit))
        {
            agent.destination = hit.position;
        }
        else
        {
            //Debug.Log("Didnt find location to take cover at");
        }
    }

    public void HelpOtherPoliceMan(Vector3 pos)
    {
        //Debug.Log("Coming to help you other policeman that i dont even know!");
        if (IsWandering())
        {
            agent.ResetPath();
            agent.destination = pos;
            ClearStates();
            movingToOtherPolice = true;
        }
    }

    private void ShootPlayer()
    {
        agent.transform.LookAt(player.transform);
        ClearStates();
        StartCoroutine(Shooting());
        StartCoroutine(FightTimer());
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(2);
        //Debug.Log("opening fire");
        shooting = true;
        while (true)
        {
            yield return new WaitForSeconds(shootingDelay);
            polShoot.Fire();
            //Debug.Log("Shot the player");
        }
    }

    IEnumerator FightTimer()
    {
        polShoot.chanceToHit = polShoot.minChanceToHit;
        int secondsInFight = 10;
        while(shooting)
        {
            yield return new WaitForSeconds(1f);
            secondsInFight++;
            if (secondsInFight % 60 == 0)
            {
                Debug.Log("I need help!");
                GameObject.FindGameObjectWithTag("Police").GetComponentInChildren<PoliceAI>().HelpOtherPoliceMan(transform.position);
            }
        }
    }
}