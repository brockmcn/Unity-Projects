using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceLookForPlayer : MonoBehaviour {

    public float lookingRange = 100f;
    public float fieldOfViewAngle = 90f;
    public float timeTillStop = 10f;
    public float radiusAroundPlayerToLook = 5f; //can also change this to go to set waypoints near player (would allow predictable movement rather than random rng and less likely for bugs to occur)
    float fovAdjustment = 0f;
    float timer = 0f;
    GameObject player;
    GameObject target;
    NavMeshAgent agent;
    PoliceChase chase;
    PolicePatrol patrol;
    bool goingToDestination;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponentInParent<NavMeshAgent>();
        chase = GetComponent<PoliceChase>();
        patrol = GetComponent<PolicePatrol>();
        target = new GameObject();
    }

    private void OnEnable()
    {
        // call look for player
        timer = 0f;
        agent.destination = new Vector3(Random.insideUnitCircle.x - .5f, 0, Random.insideUnitCircle.y - .5f) * radiusAroundPlayerToLook + player.transform.position; //probably already seen player if this is enabled
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //look for player
        //if in vision no longer looking change to chase
        //if time run out continue patrol

        if (DoesViewContainPlayer() == true)
        {
            //disable and chase = true
            chase.enabled = true;
            enabled = false;
        }

        else if(timer >= timeTillStop)
        {
            //disable and patrol = true
            patrol.enabled = true;
            GetComponent<Animator>().ResetTrigger("LookAround");
            enabled = false;
        }

        else
        {
            //keep looking
            GetComponent<Animator>().SetFloat("Speed", agent.velocity.magnitude / chase.chaseSpeed);
            if (agent.remainingDistance <= agent.stoppingDistance)//has he reached destination
            {
                if (target.transform != null) //go where player was last seen
                {
                    agent.destination = new Vector3(Random.insideUnitCircle.x - .5f, 0, Random.insideUnitCircle.y - .5f) * radiusAroundPlayerToLook + target.transform.position;
                    fovAdjustment = 45f;
                    //GetComponent<Animator>().SetTrigger("LookAround");
                    //other looking around stuff here
                }

                else
                {
                    agent.destination = new Vector3(Random.insideUnitCircle.x - .5f, 0, Random.insideUnitCircle.y - .5f) * radiusAroundPlayerToLook + agent.transform.position; //move randomly
                    fovAdjustment = 45f;
                    //GetComponent<Animator>().SetTrigger("LookAround");
                }
            }
        }
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
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), direction.normalized, out hit, lookingRange))
            {
                //Debug.Log("raycast hit " + hit.collider.gameObject);
                if (hit.collider.gameObject == player)
                {
                    target.transform.position = player.transform.position;
                    //Debug.Log("not obstructed");
                    return true;
                }
            }
        }
        return false;
    }
}
