using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    void Start()
    {
        maxHealth = transform.GetComponent<Stats>().constitution * 10 + 40;
        currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        if (currentHealth > 0)
            currentHealth -= damage;
        else
            Debug.Log("Death");
    }
}
