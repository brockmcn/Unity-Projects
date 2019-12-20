using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour {

    public Camera camera;
    public float selectionRange = 20;
    public Material redMaterial;
    public Material goldMaterial;
    public Material greenMaterial;

    public GameObject selectedTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
        {
            Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, camera.transform.forward, out hit, selectionRange))
            {
                if (hit.collider.tag == "Civilian" || hit.collider.transform.parent.tag == "Civilian" || hit.collider.transform.root.name.Equals("AIPrefabs"))
                {
                    selectedTarget = hit.collider.gameObject;
                    foreach (Transform child in hit.collider.transform)
                    {
                        child.GetComponent<Renderer>().material = goldMaterial;
                    }
                    foreach (Transform child in hit.collider.transform.parent)
                    {
                        child.GetComponent<Renderer>().material = goldMaterial;
                    }
                }
            }
        }
	}
}
