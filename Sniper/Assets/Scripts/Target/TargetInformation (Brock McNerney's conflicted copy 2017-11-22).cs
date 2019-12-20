using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetInformation : MonoBehaviour {

    public string name;
    public string info;
    public string dailyRoutine;

    public GameObject nameGO;
    public GameObject infoGO;
    public GameObject dailyRoutineGO;

    public GameObject target;

    private int longestLength;
    private GameObject longestGO;

    private bool visible = true;

	// Use this for initialization
	void Start () {
        StartSound();
        target = GameObject.FindGameObjectsWithTag("Civilian")[Random.Range(0, GameObject.FindGameObjectsWithTag("Civilian").Length - 1)];
        target.AddComponent<TargetSchedule>();

        nameGO.GetComponent<TargetInformationTextEffect>().text = name;
        infoGO.GetComponent<TargetInformationTextEffect>().text = info;
        dailyRoutineGO.GetComponent<TargetInformationTextEffect>().text = dailyRoutine;

        FindLongestThing();
    }

    private void ApplySchedule()
    {
        //apply schedule to the target
    }

    public bool IsTarget(GameObject compare)
    {
        return compare == target;
    }

    private void FindLongestThing()
    {
        string[] strings = new string[] { name, info, dailyRoutine };
        longestLength = strings.OrderByDescending(s => s.Length).First().Length;
        if (strings.OrderByDescending(s => s.Length).First() == name)
            longestGO = nameGO;
        else if (strings.OrderByDescending(s => s.Length).First() == info)
            longestGO = infoGO;
        else if (strings.OrderByDescending(s => s.Length).First() == dailyRoutine)
            longestGO = dailyRoutineGO;


        Invoke("StopSound", longestLength * longestGO.GetComponent<TargetInformationTextEffect>().delay); //buggy
        Invoke("FadeOut", longestLength * longestGO.GetComponent<TargetInformationTextEffect>().delay + 5);
    }

    private void StopSound()
    {
        GetComponent<AudioSource>().Stop();
    }

    private void StartSound()
    {
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.I))
        {
            if(visible)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }
    }

    void FadeOut()
    {
        GetComponent<Animator>().SetTrigger("Out");
        visible = false;
    }

    void FadeIn()
    {
        GetComponent<Animator>().SetTrigger("In");
        visible = true;
    }
}
