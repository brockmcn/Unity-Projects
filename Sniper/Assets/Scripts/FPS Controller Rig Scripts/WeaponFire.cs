using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour {

    // public
    public float projMuzzleVelocity; // in metres per second
    public GameObject projPrefab;
    public float RateOfFire;
    public float Inaccuracy;
    

    // private
    private float fireTimer;

    // Use this for initialization
    void Start()
    {
        fireTimer = Time.time + RateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        if (Time.time > fireTimer)
        {
            GameObject projectile;
            Vector3 muzzlevelocity = transform.forward;

            if (Inaccuracy != 0)
            {
                Vector2 rand = Random.insideUnitCircle;
                muzzlevelocity += new Vector3(rand.x, rand.y, 0) * Inaccuracy;
            }

            muzzlevelocity = muzzlevelocity.normalized * projMuzzleVelocity;

            projectile = Instantiate(projPrefab, transform.position, transform.rotation*Quaternion.Euler(90,0,0)) as GameObject;
            Debug.DrawRay(transform.position, transform.forward*10, Color.red, 10f);
            projectile.GetComponent<Projectile>().muzzleVelocity = muzzlevelocity;
            fireTimer = Time.time + RateOfFire;
        }
        else
            return;
    }
}
