//The enemy health we will have to change the updated if statment to add the ragdoll

using UnityEngine;

public class enemyHealthScript : MonoBehaviour {

    public float totalHealth = 100f;
    public float currentHealth;

	void Start ()
    {
        currentHealth = totalHealth;
	}
	
	void Update ()
    {
		if (currentHealth <= 0)
        {
            GameObject.Instantiate((GameObject)Resources.Load("Ragdoll"), gameObject.transform.parent.position, gameObject.transform.parent.rotation);
            Destroy(gameObject);
        }
	}
}