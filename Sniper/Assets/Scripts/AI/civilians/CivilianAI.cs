using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianAI : MonoBehaviour {

	public Transform[] wayPoints;
    public float distanceAwayNotScared = 200;
	Vector3 scaredPos;

    public float fieldOfViewAngle = 90f;
    public float fovAdjustment = 0f;
    public float lookingRange = 100f;

    private bool isScared;
    private NavMeshAgent navAgent;
    private static bool warningOnce = false;
    CivilianBehaviour civBehaviour;
    PolicePatrol[] police;
    bool pathSet = false;
    private GameObject player;
    private AudioSource audio;

    // Use this for initialization

    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        civBehaviour = GetComponent<CivilianBehaviour>();
        player = GameObject.Find("FPS Controller Rig Sniper Rifle");
        audio = GetComponent<AudioSource>();
        police = FindObjectsOfType<PolicePatrol>();

        audio.pitch += Random.Range(-.1f, .1f);
	}

    public bool CanSeePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle <= fieldOfViewAngle / 2 + fovAdjustment)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction.normalized, out hit, lookingRange))
            {
                if (hit.collider.gameObject == player)
                {
                    return true;
                }
            }
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {

        if (CanSeePlayer() && !GameObject.Find("Weapon Holder").GetComponent<SwitchWeapon>().noWeaponOut && !isScared)
        {
            Debug.Log("that guy has a weapon!!");
            ScareLocationSet(player.transform.position);
        }

        if (navAgent == null)
        {
            if (!warningOnce)
            {
                Debug.Log("navAgent failed, this will only display once");
                warningOnce = true;
            }
            return;
        }

		if (isScared && Vector3.Distance(scaredPos, transform.position) <= distanceAwayNotScared && pathSet == false)
		{
            navAgent.speed = 4.5f;
            GetComponentInChildren<Animator>().SetTrigger("Run");
			float i = 0;
            int x = -1;
            int arrayValue = 0;
            foreach (Transform pos in wayPoints) // finding best location depending on position
			{
				float dist = 0;
                x++;
				dist = Vector3.Distance (scaredPos, pos.position) - Vector3.Distance (transform.position, pos.position);
				if (dist >= i)
				{
                    arrayValue = x;
					dist = i;
				}
			}
            // here can also say call police
            civBehaviour.SetNextDestination(arrayValue);
            pathSet = true;
		}

		if (Vector3.Distance (scaredPos, transform.position) > distanceAwayNotScared) 
		{
            navAgent.speed = 3.5f;
            GetComponentInChildren<Animator>().SetTrigger("Walk");
            civBehaviour.scaredWaypoints = null;
			isScared = false;
		}

        //else keep doing what it normally does looping around or whatever
        //float distance = Vector3.Distance(navAgent.destination, transform.position);
        Debug.DrawLine(transform.position, navAgent.destination);
        //Debug.Log(Vector3.Distance(transform.position, player.position));
        /*if (distance < 2.0f)
        {
            Vector3 destination;
            while (true)
            {
                destination = wayPoints[Random.Range(0, wayPoints.Length - 1)];
                if (destination != transform.position) break;
            }

            NavMeshHit hit;

            NavMesh.SamplePosition(destination, out hit, 4.0f, NavMesh.AllAreas);

            navAgent.SetDestination(hit.position);
        }*/
    }

    public void ScareLocationSet(Vector3 pos) //call this when sound is made and the location of the sound will be pos;
	{
        audio.Play();
        //Debug.Log(pos);
        if (Vector3.Distance(transform.position, pos) <= distanceAwayNotScared)
        {
            civBehaviour.SetWaypoints(pos); 
            pathSet = false;
            scaredPos = pos;
            isScared = true;
            foreach(PolicePatrol pol in police)
            {
                pol.GetComponent<PolicePatrol>().ShotFired();
            }
            //call police or you can put in the update function with the relocation
        }
	}
}
