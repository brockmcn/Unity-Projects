using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth;

    [HideInInspector]
    public float currentHealth;

    PlayerStats stats;
    bool isDead = false;
    bool gameOver = false;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        maxHealth = 10 + stats.healthStat;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (currentHealth <= 0 && gameOver == false && isDead == false)
        {
            isDead = true;
            GameIsOver();
            gameOver = true;
        }

        maxHealth = 10 + stats.healthStat;
	}

    void GameIsOver()
    {
        Debug.Log("You Died");
        //restart lvl or do whatever
    }
}
