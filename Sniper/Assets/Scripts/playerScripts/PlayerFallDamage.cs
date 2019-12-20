using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDamage : MonoBehaviour {

    public float maxHeightNoFallDamage;
    public float damagePerMeterHigh;
    float lastY;
    float fallDistance;
    CharacterController controller;
    PlayerHealth health;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        health = GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastY > transform.position.y)
        {
            fallDistance += lastY - transform.position.y;
        }

        lastY = transform.position.y;

        if (fallDistance >= maxHeightNoFallDamage && controller.isGrounded)
        {
            float damageTaken;
            damageTaken = (fallDistance - maxHeightNoFallDamage) * damagePerMeterHigh;
            health.TakeDamage(damageTaken);
            ApplyNormal();
        }
        
        if(fallDistance <= maxHeightNoFallDamage && controller.isGrounded)
        {
            ApplyNormal();
        }
	}

    void ApplyNormal()
    {
        fallDistance = 0;
        lastY = 0;
    }
}
