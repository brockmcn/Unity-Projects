using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevel : MonoBehaviour {

    public int playerLevel = 1;
    public int playerEXP = 0;
    public int expToLevelUp = 100;
    public GameObject statMenu;

    public void GiveEXP(int exp)
    {
        playerEXP += exp;
        if (playerEXP >= expToLevelUp)
        {
            playerEXP -= expToLevelUp;
            LevelUp();
        }
        
    }

    void LevelUp()
    {
        statMenu.SetActive(true);
        playerLevel += 1;
    }
}
