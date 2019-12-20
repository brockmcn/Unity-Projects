using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceMovement : MonoBehaviour {

    private NavMeshAgent agent;

    public Transform[] waypoints;

    private bool isWandering = true;

    public float maxSpeed = 4.5f;

    public bool Wandering
    {
        get {
            return isWandering;
        }
        set
        {
            isWandering = value;
            if(!value)
            {
                agent.ResetPath();
            }
        }
    }

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length - 1)].position);
    }

    private void Update()
    {
        if(Wandering)
        {
            if(Vector3.Distance(agent.destination, agent.transform.position) < 2f)
            {
                agent.ResetPath();
                agent.destination = waypoints[Random.Range(0, waypoints.Length - 1)].position;
            }
        }
    }
}
