using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour {

    StaminaScript stamina;
    Slider slider;

	void Start () {
        stamina = FindObjectOfType<StaminaScript>().GetComponent<StaminaScript>();
        slider = GetComponent<Slider>();
	}
	
	void Update () {
        slider.value = stamina.currentStamina;
        slider.maxValue = stamina.maxStamina;
	}
}
