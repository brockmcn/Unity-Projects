using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoForGuns : MonoBehaviour {

    public int ammoInMagazine = 0;
    public int ammoInStock = 90;
    public int magazineSize = 15;
    public float reloadTimePerBullet = .5f;
    private bool reloading = false;

    private float reloadDone;

	// Use this for initialization
	void Start () {
        LoadFull();
        reloadDone = Time.time;
	}

    private void LoadFull()
    {
        if(ammoInStock >= magazineSize - ammoInMagazine)
        {
            ammoInStock -= magazineSize - ammoInMagazine;
            ammoInMagazine = magazineSize;
        }
        else
        {
            ammoInMagazine = ammoInStock;
            ammoInStock = 0;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammoInMagazine == magazineSize)
                return;

            if(!reloading)
            {
                reloading = true;
                if(ammoInStock >= magazineSize - ammoInMagazine)
                {
                    reloadDone = Time.time + (magazineSize - ammoInMagazine) * reloadTimePerBullet;
                }
                else
                {
                    reloadDone = Time.time + ammoInStock * reloadTimePerBullet;
                }
            }
        }
        if (reloading && Time.time >= reloadDone)
        {
            reloading = false;
            LoadFull();
        }
        GetComponentInParent<Animator>().SetBool("Reloading", reloading);
    }

    public bool UseAmmo()
    {
        if (ammoInMagazine > 0)
        {
            ammoInMagazine -= 1;
            return true;
        }else
        {
            return false;
        }
    }
}
