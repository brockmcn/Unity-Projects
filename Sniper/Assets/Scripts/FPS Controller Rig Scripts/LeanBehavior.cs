using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanBehavior : MonoBehaviour {

    public Transform _Pivot;

    public float speed = 100f;
    public float maxAngle = 20f;

    float curAngle = 0f;

    // Use this for initialization
    void Awake()
    {
        if (_Pivot == null && transform.parent != null) _Pivot = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        // lean left
        if (Input.GetKey(KeyCode.Q))
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, maxAngle, speed * Time.deltaTime);
        }
        // lean right
        else if (Input.GetKey(KeyCode.E))
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, -maxAngle, speed * Time.deltaTime);
        }
        // reset lean
        else
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, speed * Time.deltaTime);
        }

        _Pivot.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
    }

}
