using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private int gold;

    void Start()
    {
        gold = Random.Range(50, 101);
    }

    public int getGold()
    {
        int tmpGold = gold;
        gold = 0;
        return tmpGold;
    }
}
