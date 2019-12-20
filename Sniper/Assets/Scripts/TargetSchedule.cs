using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetSchedule : MonoBehaviour {

    public GameObject[] waypoints;
    public float[] times;
    public float[] timesToStay;

    private float leave;
    private int currentIndex;

    private NavMeshAgent agent;
    private CivilianBehaviour behaviour;
    private GameTime gameTime;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        behaviour = GetComponent<CivilianBehaviour>();
        gameTime = FindObjectOfType<GameTime>().GetComponent<GameTime>();
        if (agent == null)
            agent.GetComponentInParent<NavMeshAgent>();
        if (behaviour == null)
            behaviour = GetComponentInParent<CivilianBehaviour>();

        behaviour.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        float time = ((float)gameTime.time.Hour + ((float)gameTime.time.Minute * 0.01f));
        //Debug.Log(Time.time);
        //Debug.Log(((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f)));
        if (times != null)
        {
            for (int i = 0; i < times.Length; i++)
            {
                if (time > times[i] && time < times[i + 1])
                {
                    if (currentIndex != i)
                    {
                        leave = time + timesToStay[i];
                        currentIndex = i;
                    }
                }
            }
        }
        if (agent != null && waypoints != null)
        {
            agent.SetDestination(waypoints[currentIndex].transform.position);
        }
	}
}
