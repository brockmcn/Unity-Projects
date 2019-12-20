using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthBehaviour : MonoBehaviour {

    [Range(0.00f, 1.00f)]
    public float currentStealth;
    public float timeToWaitUntilPoliceStopLooking;
    public bool isSeen = false;
    GameObject wayPointAtPlayer;
    float clock;

	// Use this for initialization
	void Start () {
		//set chance of getting caught related to the stealth stat if you want to change it a bit
	}
	
	// Update is called once per frame
	void Update () {
		if (isSeen == true)
        {
            clock += Time.deltaTime;
            if(wayPointAtPlayer != null)
            {
                //move waypoint to player loction
                wayPointAtPlayer.transform.position = transform.position;
            }

            else
            {
                //create waypoint at player location
                wayPointAtPlayer = new GameObject();
                wayPointAtPlayer.transform.position = transform.position;
            }

            if (clock >= timeToWaitUntilPoliceStopLooking)
            {
                isSeen = false;
            }
        }
	}

    public void StealthOrCaught(float alertnessPerMeter, Vector3 witnessPos)
    {
        //alertness of ai = increase means higher chance of getting caught
        //greater stealth means less chance of getting caught
        //closer to target means higher chance

        float dist = Vector3.Distance(transform.position, witnessPos);
        float alertness = alertnessPerMeter / dist;
        float total = alertness + currentStealth;
        if (Random.Range(0, total) > currentStealth)
        {
            isSeen = true;
            clock = 0f;
        }
    }
}
