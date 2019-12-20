using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour {

    private GameObject player;
    public bool canClimb;
    public float climbSpeed = 0.5f;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (canClimb && Input.GetKey(KeyCode.W))
        {
			player.transform.position += Vector3.up * climbSpeed * Time.deltaTime;
        }

		if (canClimb && Input.GetKey(KeyCode.S) && !player.GetComponent<CharacterController>().isGrounded)
		{
			player.transform.position -= Vector3.up * climbSpeed* Time.deltaTime;
		}
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
        {
            canClimb = true;
            player.GetComponent<FPSController>().applyPhysics = false;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        canClimb = false;
        player.GetComponent<FPSController>().applyPhysics = true;
    }
}
