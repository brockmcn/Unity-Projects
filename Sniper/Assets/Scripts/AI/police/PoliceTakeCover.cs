using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceTakeCover : MonoBehaviour {

    public float moveSpeed;
    PoliceShoot shoot;
    PoliceChase chase;
    NavMeshAgent agent;
    float speed;
    bool goingToCover;

    private void Awake()
    {
        shoot = GetComponent<PoliceShoot>();
        agent = GetComponentInParent<NavMeshAgent>();
        speed = agent.speed;
        chase = GetComponent<PoliceChase>();
    }


    private void OnEnable()
    {
        GoToCover();
    }

    void GoToCover()
    {
        goingToCover = true;
        NavMeshHit hit;
        if(agent.FindClosestEdge(out hit))
        {
            agent.destination = hit.position;
            agent.speed = moveSpeed;
        }
    }
    private void Update()
    {
        //on destination reached shoot at player again and never do this until patrolling again
        GetComponent<Animator>().SetFloat("Speed", agent.velocity.magnitude / chase.chaseSpeed);
        if (agent.remainingDistance <= agent.stoppingDistance && goingToCover == true)
        {
            //shooting back to normal
            shoot.enabled = true;
            agent.speed = speed;
            enabled = false;
        }
    }

    private void OnDisable()
    {
        shoot.inCover = true;
        goingToCover = false;
    }
}
