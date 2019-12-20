using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    public int increaseStatByThisMuch;

    public bool isHealthStatButton;
    public bool isStaminaStatButton;
    public bool isPersuasionStatButton;
    public bool isStealthStatButton;

    PlayerStats stats;


    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>();
    }

    public void IncreaseStat()
    {
        if (isHealthStatButton == true)
        {
            stats.IncreaseHealthStat(increaseStatByThisMuch);
        }
        else if (isStaminaStatButton == true)
        {
            stats.IncreaseStaminaStat(increaseStatByThisMuch);
        }
        else if (isPersuasionStatButton == true)
        {
            stats.IncreasePersuasionStat(increaseStatByThisMuch);
        }
        else if (isStealthStatButton == true)
        {
            stats.IncreaseStealthStat(increaseStatByThisMuch);
        }
    }


}
