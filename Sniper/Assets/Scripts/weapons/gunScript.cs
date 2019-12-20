using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour {

    AmmoForGuns ammo;

    public bool isSniper;
    public GameObject ammunition;
    public Transform barrelStart;
    public Transform barrelEnd;

    private AudioSource audioSource;
    public AudioClip fire;
    public AudioClip cock;
    public AudioClip reload;

    private Animator anim;

    public bool isBoltAction;

    public float fireRate = 1;
    public float cockDelay = 0.25f;

    public float runAccuracy = 10f;
    public float walkAccuracy = 5f;
    public float StillAccuracy = 1f;
    public float zoomAccuracy = 0.5f;
    [HideInInspector]
    public float currentAccuracy = 0.5f;
    private bool canFire = true;

    private SwitchWeapon switchWeapon;
    public Camera fpsCam;
    private FPSController fpsControl;

    private CivilianAI[] civScript;
    private PoliceAI[] polScript;
    private PolicePatrol[] polPatrol;

    public void Fire()
    {
        if (fpsControl.movementStatus == PlayerMoveStatus.Running)
        {
            currentAccuracy = runAccuracy;            
        }

        else if (fpsControl.movementStatus == PlayerMoveStatus.Walking)
        {
            currentAccuracy = walkAccuracy;
        }

        else if(fpsControl.movementStatus == PlayerMoveStatus.NotGrounded)
        {
            currentAccuracy = runAccuracy;
        }

        else if (fpsControl.movementStatus == PlayerMoveStatus.NotMoving || fpsControl.movementStatus == PlayerMoveStatus.Crouching)
        {
            currentAccuracy = StillAccuracy;
        }

        else
        {
            currentAccuracy = zoomAccuracy;
        }
        //Debug.Log(canFire);
        //check if the gun is ready to fire first
        if (canFire) {

            if(!ammo.UseAmmo())
            {
                return;
            }

            //get shoot direction
            Vector3 relativePos = barrelEnd.position - barrelStart.position;
            Quaternion bulletRotation = Quaternion.LookRotation(relativePos);

            Vector3 bulletOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)) + new Vector3(Random.Range(-currentAccuracy,currentAccuracy)/100, Random.Range(-currentAccuracy, currentAccuracy)/100, 0f);

            GameObject.Instantiate(ammunition, bulletOrigin, fpsCam.transform.rotation) ; //fire the bullet

            civScript = FindObjectsOfType<CivilianAI>();
            polScript = FindObjectsOfType<PoliceAI>();
            polPatrol = FindObjectsOfType<PolicePatrol>();

            foreach (CivilianAI civ in civScript)
            {
                civ.GetComponent<CivilianAI>().ScareLocationSet(transform.position); //alert all the civillians
            }

            foreach (PoliceAI pol in polScript)
            {
                pol.GetComponent<PoliceAI>().ShotFired(); //alert all the police men
            }

            foreach (PolicePatrol pol in polPatrol)
            {
                pol.GetComponent<PolicePatrol>().ShotFired(); //alert all the police men
            }

            //set up the wait for the next shot
            StartCoroutine("GunCooldown");
        }
    }



	// Use this for initialization
	void Start () {
        ammo = transform.GetComponent<AmmoForGuns>();
        audioSource = transform.GetComponent<AudioSource>();
        anim = transform.parent.GetComponent<Animator>();
        switchWeapon = GetComponentInParent<SwitchWeapon>();
        fpsControl = GetComponentInParent<FPSController>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(barrelEnd.position, barrelStart.position,Color.red);
    }

    private IEnumerator GunCooldown()
    {
        audioSource.PlayOneShot(fire);

        anim.Play("Shooting");

        switchWeapon.canSwitchWeapons = false;

        canFire = false;
        if (isBoltAction)
        {
            yield return new WaitForSeconds(cockDelay);
            audioSource.PlayOneShot(cock);
        }
        yield return new WaitForSeconds(fireRate);
        switchWeapon.canSwitchWeapons = true;
        canFire = true;
    }
}
