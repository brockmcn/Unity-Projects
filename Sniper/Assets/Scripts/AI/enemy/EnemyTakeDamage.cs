using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour {

    EnemyHealth health;

	// Use this for initialization
	void Start () {
        health = GetComponent<EnemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void recieveDamage ()
    {
        health.Damage(1);
    }

}
