using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {

    public DateTime time;

	// Use this for initialization
	void Start () {
        time = new DateTime();
        StartCoroutine(Thread());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Thread()
    {
        while(true) //timer running at 3 hours per minute realtime
        {
            yield return new WaitForSeconds(.5f);
            time = time.AddSeconds(10);
        }
    }
}
