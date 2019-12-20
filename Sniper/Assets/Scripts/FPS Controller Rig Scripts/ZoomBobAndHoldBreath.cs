using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomBobAndHoldBreath : MonoBehaviour
{

    public float speed = 0.7f;
    float maxRotationy = 1f;
    float maxRotationx = 1f;
    public float amplitude;
    public bool holdBreath = true;

    private float timeElapsed = 0f;

    void OnEnable()
    {
        timeElapsed = Time.time; // set to Time.time or any random value
        //UpdateRotation();
    }

    void OnDisable()
    {
        transform.localRotation = Quaternion.identity; // re-center camera when zoomed out
    }

    void Update()
    {
        if (!holdBreath)
            timeElapsed += Time.deltaTime; // only increase elapsed time if not holding breath

        UpdateRotation();

        //added, have to enable when zoomed and disable when not zoomed

        if (Input.GetButtonDown("Fire2"))
        {
            OnEnable();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            OnDisable();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            holdBreath = !holdBreath;
        }

        //endadded
    }

    void UpdateRotation()
    {
        transform.localRotation = Quaternion.Euler(maxRotationx * Mathf.Sin(timeElapsed * speed * 2) * amplitude, maxRotationy * Mathf.Sin(timeElapsed * speed), 0f);

        if (transform.localRotation.eulerAngles.x <= 0.005f && transform.rotation.eulerAngles.x >= -0.005f)
        {
            amplitude = Random.Range(0.1f, 0.5f);
        }
    }

}
