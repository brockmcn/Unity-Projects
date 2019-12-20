using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile2 : MonoBehaviour {

    public Vector3 windSpeed;

    public float velocity; //muzzle velocity ft/s

    //used to calculate collision
    private Vector3 lastPosition;
    private Vector3 currentPosition;

    public GameObject bulletHolePrefab;

    private Rigidbody rb;

    //for projectile despawning
    private float ttl = 1;
    private double startTime;

	// Use this for initialization
	void Start () {

        //get rigidbody
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * (rb.mass*velocity*50)*0.3048f); //add starting force to propel to muzzle velocity

        lastPosition = transform.position;
        currentPosition = transform.position;

        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void FixedUpdate()
    {
        //add wind
        rb.AddForce(windSpeed);

        //calculate collision
        currentPosition = transform.position;
        RaycastHit hit; Debug.DrawLine(lastPosition, currentPosition);
        if (Physics.Linecast(lastPosition, currentPosition, out hit))
        {
            //get gameobject of collided object
            GameObject colObj = hit.collider.gameObject;

            //if collided object has a rigidbody, apply force to it
            Rigidbody colRb = colObj.GetComponent<Rigidbody>();
            if (colRb)
            {
                colRb.AddForceAtPosition(new Vector3(   transform.forward.x * rb.velocity.x, 
                                                        transform.forward.y * rb.velocity.y, 
                                                        transform.forward.z * rb.velocity.z) * rb.mass,
                                                        transform.position);
            }

            //send damage to the object
            colObj.SendMessage("recieveDamage");
            if (colObj.tag == "Untagged")
            {
                Instantiate(bulletHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }

            Destroy(gameObject);
        }
        lastPosition = transform.position;

        //if velocity too low, destroy the gameobject
        if(rb.velocity.z < 50 && Time.time - startTime > ttl)
        {
            GameObject.Destroy(gameObject);
            Debug.Log("destroyed");
        }
    }
}
