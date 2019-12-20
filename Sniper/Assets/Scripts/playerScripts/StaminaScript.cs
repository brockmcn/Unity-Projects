using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaScript : MonoBehaviour {

    public float maxStamina;
    public float staminaRegen;
    public float timeTillRegen;

    [HideInInspector]
    public float currentStamina;

    PlayerStats stats;
    bool staminaUse = false;
    
    float useStamina;
    float timeKeep;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        maxStamina = 10 + stats.staminaStat; //gotta fix this make it so default max stamina can be changed
        currentStamina = maxStamina;
    }

    public void UseStamina(float costPerSecond)
    {
        staminaUse = true;
        useStamina = costPerSecond;
    }

    public void StoppedUsingStamina()
    {
        //staminaUse = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeKeep += Time.deltaTime;
        if (staminaUse == true && currentStamina > 0)
        {
            currentStamina -= useStamina * Time.deltaTime;
            timeKeep = 0;
        }
        if (currentStamina <= maxStamina && timeTillRegen <= timeKeep)
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
        }

        maxStamina = 10 + stats.staminaStat; // same here
        staminaUse = false;
        //Debug.Log(currentStamina);
    }
}
