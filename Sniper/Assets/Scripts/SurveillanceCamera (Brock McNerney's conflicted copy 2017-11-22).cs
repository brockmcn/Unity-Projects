using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour {

    public GameObject player;

    public float lookingRange = 100f;
    public float horizontalFOV = 90f;
    public float verticalFOV = 90f;

    public float alertionRange = 100f;

    public float smoothing = .1f;
    public float moveDelay = 3f;

    public bool isLockedOn = false;

    private Quaternion nextRotation;

	// Use this for initialization
	void Start () {
        StartCoroutine(RandomMovement());
	}
	
	// Update is called once per frame
	void Update () {
        if (isLockedOn)
        {
            transform.LookAt(player.transform);
        }
        else {
            Vector3 viewport = transform.Find("CameraSight").GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
            if (viewport.x > 0 && viewport.x < 1 && viewport.y > 0 && viewport.y < 1 && viewport.z > 0 && !isLockedOn)
            {
                Debug.Log("can see");
                if (!GameObject.Find("Weapon Holder").GetComponent<SwitchWeapon>().noWeaponOut)
                {
                    Debug.Log("has weapon");
                    isLockedOn = true;
                    StopCoroutine("RandomMovement");
                    foreach(GameObject go in GameObject.FindGameObjectsWithTag("Police"))
                    {
                        if(Vector3.Distance(gameObject.transform.position, go.transform.position) <= alertionRange)
                        {
                            go.transform.Find("PoliceMain").GetComponent<PolicePatrol>().ShotFired();
                        }
                    }
                }
            }
            transform.localRotation = Quaternion.Lerp(transform.localRotation, nextRotation, smoothing);
        }
    }

    private void LateUpdate()
    {
        if(isLockedOn)
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 90);
    }

    IEnumerator RandomMovement()
    {
        while(true)
        {
            nextRotation = Quaternion.Euler(Random.Range(-25f, 50f), Random.Range(-75, 75), 90f);
            yield return new WaitForSeconds(moveDelay);
        }
    }
}
