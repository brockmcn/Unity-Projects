//Put this one the each part of body that can be hit
//Make sure it has a collider
//The damage modifier is used to determine how much damage the bullet does to that certain body part think in terms of %of total damage

using UnityEngine;

public class limbDamage : MonoBehaviour {

    public float damageModifier;
    private float bulletDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "bullet")
        {
            GameObject bullet = GameObject.FindGameObjectWithTag("bullet");
            bulletScript bulletScript = bullet.GetComponent<bulletScript>();

            bulletDamage = bulletScript.damage;

            DamageEnemy();
        }
    }

    void DamageEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("enemy");
        enemyHealthScript enemyHealthScript = player.GetComponent<enemyHealthScript>();
        enemyHealthScript.currentHealth -= bulletDamage * damageModifier;

        Debug.Log("Enemy Health: " + enemyHealthScript.currentHealth);
    }
}
