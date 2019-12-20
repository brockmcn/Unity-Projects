using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ScopeBob2 : MonoBehaviour {

    public bool isHoldingBreath = false;
    public bool isZoomed = false;

    StaminaScript staminaScript;

    // Use this for initialization
    void Start () {
        staminaScript = FindObjectOfType<StaminaScript>().GetComponent<StaminaScript>();
    }

    // Update is called once per frame
    void Update () {
        if (!isZoomed)
        {
            GetComponent<Animator>().SetBool("IsBreathing", false);
        }
        else
        {
            if(!isHoldingBreath && isZoomed)
                GetComponent<Animator>().SetBool("IsBreathing", true);
        }
        if (Input.GetKey(KeyCode.Tab) && isZoomed)
        {
            if (staminaScript.currentStamina > 0)
            {
                GetComponent<Animator>().SetBool("IsBreathing", false);
                isHoldingBreath = true;
                staminaScript.UseStamina(2);
            }
            else
            {
                GetComponent<Animator>().SetBool("IsBreathing", true);
                isHoldingBreath = false;
                //staminaScript.StoppedUsingStamina();
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab) && isZoomed)
        {
            //staminaScript.StoppedUsingStamina();
            isHoldingBreath = false;
            GetComponent<Animator>().SetBool("IsBreathing", true);
        }
    }
}
