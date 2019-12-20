using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTextUI : MonoBehaviour {

    public bool isHealthStatText;
    public bool isStaminaStatText;
    public bool isPersuasionStatText;
    public bool isStealthStatText;

    PlayerStats stats;
    Text text;

    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (isHealthStatText == true)
        {
            text.text = "Health: " + stats.healthStat;
        }
        else if (isStaminaStatText == true)
        {
            text.text = "Stamina: " + stats.staminaStat;
        }
        else if (isPersuasionStatText == true)
        {
            text.text = "Persuasion: " + stats.persuasionStat;
        }
        else if (isStealthStatText == true)
        {
            text.text = "Stealth: " + stats.stealthStat;
        }
    }


}
