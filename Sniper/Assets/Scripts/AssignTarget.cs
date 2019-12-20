using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignTarget : MonoBehaviour
{
    public Material mat;
    public GameObject[] targetParts;
    public float sightRange = 50;
    private Camera fpsCam;

    void Start()
    {
        fpsCam = GetComponent<Camera>();
    }

    void Update()
    {

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, sightRange))
        {
            foreach (GameObject targetPart in targetParts)
            {
                if (hit.collider.name == targetPart.name)
                {
                    for (int i = 0;  i < targetParts.Length; i++)
                        targetParts[i].GetComponent<Renderer>().material = mat;
                }
            }
        }
    }
}