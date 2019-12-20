using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour {

    private bool isShooting;
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f; //if we want to like move/break stuff
    public Transform gunEnd;
    public bool gunShot;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f); // here we can add like animations for sniping or whatever just make it longer to fit animation timing
    private AudioSource gunAudio;
    private LineRenderer bulletLine;
    private float nextFire; //timer

    void Start () {
        bulletLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
	}

    public Animator animator;

    void Update () {

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
        {
            gunShot = true;
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            bulletLine.SetPosition(0, gunEnd.position);

            animator.Play("Shooting");
            isShooting = true;

        if (Physics.Raycast (rayOrigin,fpsCam.transform.forward,out hit, weaponRange))
            {
                bulletLine.SetPosition(1, hit.point);

                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

                if ( enemyHealth != null)
                {
                    enemyHealth.Damage(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }

            }

        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isShooting = false;
            animator.SetBool("Shooting", isShooting);
        }

    }

    private IEnumerator ShotEffect() // here you can play animation or whatever for the shooting
    {
        gunAudio.Play();

        bulletLine.enabled = true;

        yield return shotDuration;

        bulletLine.enabled = false;
    }
}
