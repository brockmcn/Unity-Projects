using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour {

    private bool shown = false;
    private Vector3 desiredPos = new Vector3(-0.101f, -0.37f, 0.27f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.G))
        {
            desiredPos = shown ? new Vector3(-0.101f, -0.37f, 0.27f) : new Vector3(-0.101f, -0.085f, 0.27f);
            shown = !shown;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, .1f);
	}
}
