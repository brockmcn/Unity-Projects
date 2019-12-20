using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceShoot : MonoBehaviour {

    public float gunRange = 100;
    public float timeTillBackupCalled = 10;
    public float initialCoverRange = 10;
    public float maxCoverRange = 50;
    public float rateOfCoverRangeIncrease = 1;

    GameObject player;
    PoliceTakeCover cover;
    PoliceCallForBackUp backup;
    PoliceChase chase;
    PoliceShooting shooting;
    AimAtPlayer aim;
    NavMeshAgent agent;
    Animator animator;

    public bool backupCalled = false;
    public bool inCover = false;
    private bool shootingAnim = false;

    float timer = 0f;
    float currentCoverRange = 10;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        cover = GetComponent<PoliceTakeCover>();
        backup = GetComponent<PoliceCallForBackUp>();
        chase = GetComponent<PoliceChase>();
        agent = GetComponentInParent<NavMeshAgent>();
        shooting = GetComponentInChildren<PoliceShooting>();
        aim = GetComponentInChildren<AimAtPlayer>();
        currentCoverRange = 10;
        backupCalled = false;
        inCover = false;
	}

    private void OnEnable()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update ()
    {
        animator.SetBool("Shooting", shootingAnim);
        //look at player and shoot
        //if too far switch to chase mode
        //after a certain amount of time call back up once then start shooting again
        //if there is cover within shooting range of player and within range of police, police will choose to go to cover if not already in cover and shoot
        timer += Time.deltaTime;
        currentCoverRange += rateOfCoverRangeIncrease * Time.deltaTime;
        if (gunRange <= Vector3.Distance(transform.position, player.transform.position))
        {
            //chase mode
            chase.enabled = true;
            shootingAnim = false;
            aim.firing = false;
            enabled = false;
        }

        if (timer >= timeTillBackupCalled && backupCalled == false)
        {
            //call for backup once
            //call for backup animation here
            //backup.enabled = true;
            timer = 0;
            aim.firing = false;
            //shootingAnim = false;
            backupCalled = true;
            //enabled = false;
        }
        NavMeshHit hit;
        if (agent.FindClosestEdge(out hit)) //find nearest cover (should change it to waypoints that specify it is cover when we do make cover)
        {
            if(currentCoverRange >= Vector3.Distance(transform.position,hit.position) && gunRange >= Vector3.Distance(player.transform.position, hit.position) && inCover == false)
            {
                //move to the cover
                agent.isStopped = false;
                cover.enabled = true;
                shootingAnim = false;
                inCover = true;
                aim.firing = false;
                enabled = false;
            }
        }

        if (gunRange >= Vector3.Distance(transform.position, player.transform.position) && shooting.canFire == true)
        {
            //here you can add timers to tell it when to pop out and such
            shootingAnim = true;
            agent.transform.LookAt(player.transform);
            aim.firing = true;
            if (timer >= 2.5f)
            {
                shooting.Fire();
            }
        }

        else if(shooting.canFire == false)
        {
            
        }
	}

    private void OnDisable()
    {
        animator.SetBool("Shooting", shootingAnim);
        shootingAnim = false;
        aim.firing = false;
    }
}
