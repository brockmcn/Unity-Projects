using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    PlayerHealth health;
    Slider slider;

	// Use this for initialization
	void Start () {
        health = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();
        slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = health.currentHealth;
        slider.maxValue = health.maxHealth;
	}
}
