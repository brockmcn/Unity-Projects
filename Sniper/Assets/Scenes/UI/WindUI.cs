using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindUI : MonoBehaviour
{
    float imageRotate;
    Wind wind;
    Slider slider;

    void Start()
    {
        wind = FindObjectOfType<Wind>().GetComponent<Wind>();
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, wind.xWind, 1 * Time.deltaTime);
    }
}