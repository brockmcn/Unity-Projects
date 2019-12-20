using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShooting : MonoBehaviour {

    AmmoForGuns ammo;

    public bool isSniper;
    public GameObject ammunition;
    public Transform barrelStart;
    public Transform barrelEnd;
    private Transform player;

    private AudioSource audioSource;
    public AudioClip fire;
    public AudioClip cock;
    public AudioClip reload;

    private Animator anim;

    public bool isBoltAction;

    public float fireRate = 3;
    public float cockDelay = 0.25f;
    public bool canFire = true;

    public float minChanceToHit = 10;
    public float chanceToHit = 10;
    public float chanceIncreaseAfterEachShot = 10;
    public float maxChance = 80;
    private bool hit = false;


	// Use this for initialization
	void Start () {
        ammo = transform.GetComponent<AmmoForGuns>();
        audioSource = transform.GetComponent<AudioSource>();
        anim = transform.parent.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        chanceToHit = minChanceToHit;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.DrawLine(barrelStart.position, player.position, Color.blue);
	}

    public void Fire()
    {
        //Debug.Log(canFire);
        //check if the gun is ready to fire first
        if (canFire)
        {
            float randNum = Random.Range(0, 1);
            if (randNum <= chanceToHit / 100)
            {
                hit = true;
                //Debug.Log("hit");
                if (chanceToHit <= maxChance)
                {
                    chanceToHit += chanceIncreaseAfterEachShot;
                }
            }
            else
            {
                hit = false;
                if (chanceToHit <= maxChance)
                {
                    chanceToHit += chanceIncreaseAfterEachShot;
                }
            }
            //get shoot direction
            if (hit == true)
            {
                Vector3 relativePos = player.position - barrelStart.position;
                Quaternion bulletRotation = Quaternion.LookRotation(relativePos);

                GameObject.Instantiate(ammunition, barrelEnd.position, bulletRotation); //fire the bullet

                ammo.UseAmmo(); //ammo is used
                //set up the wait for the next shot
                StartCoroutine("GunCooldown");
                hit = false;
            }
            else
            {
                //here you can make the bullet fire where ever or play an animation of bullet flying
                ammo.UseAmmo(); //ammo is used
                //set up the wait for the next shot
                StartCoroutine("GunCooldown");
                hit = false;
            }
        }
    }

    private IEnumerator GunCooldown()
    {
        audioSource.PlayOneShot(fire);

        //animations here
        //reload animation here
        //crouch animation (shooting = false)
        canFire = false;
        if (isBoltAction)
        {
            yield return new WaitForSeconds(cockDelay);
            audioSource.PlayOneShot(cock);
        }
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}

