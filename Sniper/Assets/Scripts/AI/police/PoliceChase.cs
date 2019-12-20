using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceChase : MonoBehaviour {

    public float chaseSpeed = 4.5f;
    public float gunRange = 100f;
    public float animationLength;
    GameObject player;
    PoliceLookForPlayer lookForPlayer;
    PoliceShoot shoot;
    NavMeshAgent agent;
    bool animationPlaying = false;
    float speed;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lookForPlayer = GetComponent<PoliceLookForPlayer>();
        shoot = GetComponentInChildren<PoliceShoot>();
        agent = GetComponentInParent<NavMeshAgent>();
        speed = agent.speed;
    }

    private void OnEnable()
    {
        agent.destination = player.transform.position;//chase player
        agent.speed = chaseSpeed;
        agent.isStopped = false;
        GetComponent<Animator>().SetTrigger("Found");
    }

    private void Update()
    {
        GetComponent<Animator>().SetFloat("Speed", agent.velocity.magnitude / chaseSpeed);
        if (lookForPlayer.DoesViewContainPlayer() == true && Vector3.Distance(transform.position, player.transform.position)>= gunRange)
        {
            agent.destination = player.transform.position;//chase player
            agent.speed = chaseSpeed;
            if (animationPlaying == false)
            {
                PlayAnimation();
            }
        }

        else if (lookForPlayer.DoesViewContainPlayer() == true && Vector3.Distance(transform.position, player.transform.position) <= gunRange)
        {
            //shoot player
            agent.isStopped = true;
            shoot.enabled = true;
            agent.speed = speed;
            GetComponent<Animator>().ResetTrigger("Found");
            enabled = false;
        }

        else if (lookForPlayer.DoesViewContainPlayer() == false)
        {
            //look for player
            GetComponent<Animator>().ResetTrigger("Found");
            lookForPlayer.enabled = true;
            agent.speed = speed;
            enabled = false;
        }
    }

    IEnumerator PlayAnimation()
    {
        animationPlaying = true;
        //play animation here
        GetComponent<Animator>().SetTrigger("Found");
        yield return new WaitForSeconds(animationLength);
        animationPlaying = false;
    }

}
