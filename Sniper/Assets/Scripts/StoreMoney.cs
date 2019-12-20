using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMoney : MonoBehaviour {

    public int currentMoney;



	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("Money", 0);
        currentMoney = PlayerPrefs.GetInt("Money");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UseMoney(int cost)
    {
        if (currentMoney - cost >= 0)
        {
            currentMoney -= cost;
            PlayerPrefs.SetInt("Money", currentMoney);
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

    public void GainMoney(int gain)
    {
        currentMoney += gain;
        PlayerPrefs.SetInt("Money", currentMoney);
    }
}
