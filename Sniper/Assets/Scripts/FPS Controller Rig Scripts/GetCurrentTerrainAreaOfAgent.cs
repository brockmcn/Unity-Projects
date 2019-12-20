using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GetCurrentTerrainAreaOfAgent : MonoBehaviour {

    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        int waterMask = 1 << NavMesh.GetAreaFromName("Water");// 1 << means shift 1 to the left by the number of bits specified by navmesh.getareafromname("water")
        //Debug.Log("area from name: " + NavMesh.GetAreaFromName("Water"));
        
        //Debug.Log("watermask " + waterMask);

        NavMeshHit hit;
        agent.SamplePathPosition(-1, 0.0f, out hit);
        //Debug.Log("hit mask: " + hit.mask);
        //if (hit.mask == waterMask)
            //Debug.Log("In The Water");
        //else
          //  Debug.Log("On Land");
    }
}
