using UnityEngine;

public class fireGun : MonoBehaviour {

    public Rigidbody projectile;
    public float bulletDamage;
    public float velocity;

	void Update ()
    {
		if(Input.GetButtonDown("Fire1"))
        {
            Rigidbody bulletClone;
            bulletClone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
            bulletClone.velocity = transform.TransformDirection(Vector3.forward * velocity);

            GameObject bullet = GameObject.FindGameObjectWithTag("bullet");
            bulletScript bulletScript = bullet.GetComponent<bulletScript>();
            bulletScript.damage = bulletDamage;
        }
	}
}
