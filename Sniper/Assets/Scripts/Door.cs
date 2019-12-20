using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    private bool isMouseOver = false;

    public bool open = false;
    public float smoothing = 0.1f;
    public float interactDistance = 5f;

    private Quaternion offset;

	// Use this for initialization
	void Start () {
        offset = transform.parent.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E) && isMouseOver && Vector3.Distance(GameObject.Find("FPS Controller Rig Sniper Rifle").transform.position, gameObject.transform.position) < interactDistance)
        {
            open = !open;
        }
        transform.parent.GetComponent<Animator>().SetBool("Open", open);
        if (Vector3.Distance(GameObject.Find("FPS Controller Rig Sniper Rifle").transform.position, gameObject.transform.position) < interactDistance && isMouseOver)
        {
            GameObject.Find("InteractionInfo").GetComponent<Text>().text = "Press [E] to open door";
        }else if(Vector3.Distance(GameObject.Find("FPS Controller Rig Sniper Rifle").transform.position, gameObject.transform.position) > interactDistance && isMouseOver)
        {
            GameObject.Find("InteractionInfo").GetComponent<Text>().text = "";
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;

    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        GameObject.Find("InteractionInfo").GetComponent<Text>().text = "";
    }

    private void LateUpdate()
    {
        transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles + offset.eulerAngles);
    }
}
