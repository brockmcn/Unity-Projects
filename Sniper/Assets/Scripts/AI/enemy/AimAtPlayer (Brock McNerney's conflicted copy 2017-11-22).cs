using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour {

    private GameObject target;
    private Vector3 targetPoint;
    private Quaternion targetRotation;
    public bool firing = false;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        firing = false;
    }

    void Update()
    {
        if (firing == true)
        {
            targetPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            targetRotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
        }
    }
}
