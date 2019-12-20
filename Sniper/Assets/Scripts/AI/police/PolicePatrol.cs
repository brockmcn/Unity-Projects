using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PolicePatrol : MonoBehaviour {

    private NavMeshAgent agent;

    public Transform[] waypoints;
    public float hearingRange = 100f;

    GameObject player;
    PoliceChase chase;
    PoliceLookForPlayer lookForPlayer;
    bool lookingForPlayer = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponentInParent<NavMeshAgent>();
        lookForPlayer = GetComponent<PoliceLookForPlayer>();
        chase = GetComponent<PoliceChase>();
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length - 1)].position);
        lookingForPlayer = false;
    }

    private void OnEnable()
    {
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length - 1)].position);
        lookingForPlayer = false;
    }

    private void Update()
    {
        GetComponent<Animator>().SetFloat("Speed", agent.velocity.magnitude / chase.chaseSpeed);
        if (lookingForPlayer == true)
        {
            lookForPlayer.enabled = true;
            enabled = false;
        }
        if (agent.remainingDistance <= agent.stoppingDistance) // random waypoints patrolling
        {
            agent.isStopped = false;
            agent.ResetPath();
            agent.destination = waypoints[Random.Range(0, waypoints.Length - 1)].position;
        }

        //if player pulls out gun within looking distance chase player
        //if gunshot is heard look for player near gunshot position

        if (lookForPlayer.DoesViewContainPlayer() && !GameObject.Find("Weapon Holder").GetComponent<SwitchWeapon>().noWeaponOut)
        {
            chase.enabled = true;
            enabled = false;
        }
        lookingForPlayer = false;
    }

    public void ShotFired()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= hearingRange)
        {
            //look for player
            lookingForPlayer = true;
        }
    }
}
