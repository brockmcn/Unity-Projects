using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public int allHealth = 3;
    public int currentHealth;


    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        allHealth--;

        if (currentHealth <= 0 || allHealth <= 0)
        {
            GetComponentInParent<EnemyDeath>().Death();
        }
    }
}
