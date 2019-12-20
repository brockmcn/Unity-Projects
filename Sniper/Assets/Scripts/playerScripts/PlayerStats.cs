using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int healthStat;
    public int staminaStat;
    public int persuasionStat;
    public int stealthStat;

    public void IncreaseHealthStat(int increaseStat)
    {
        healthStat += increaseStat;
    }

    public void IncreaseStaminaStat(int increaseStat)
    {
        staminaStat += increaseStat;
    }

    public void IncreasePersuasionStat(int increaseStat)
    {
        persuasionStat += increaseStat;
    }

    public void IncreaseStealthStat(int increaseStat)
    {
        stealthStat += increaseStat;
    }

}
