using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    Scope scope;
    public GameObject currentWeapon;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public bool noWeaponOut;
    public bool canSwitchWeapons = true;
    public float switchingDelay = .1f;

    float nextSwitch;
    bool primaryWeaponOut;
    bool secondaryWeaponOut;
    int weapon = 0;

    private void Start()
    {
        primaryWeapon.SetActive(false);
        secondaryWeapon.SetActive(false);
        noWeaponOut = true;
        canSwitchWeapons = true;
        currentWeapon = secondaryWeapon; //currentWeapon is only used to determine if you can scope or not
        scope = transform.GetComponent<Scope>();
        nextSwitch = Time.time + switchingDelay;
    }

    IEnumerator SwitchToPrimary()
    {
        GetComponent<Animator>().SetTrigger("SwitchWeapon");
        currentWeapon = primaryWeapon;
        yield return new WaitForSeconds(.66f);
        primaryWeapon.SetActive(true);
        secondaryWeapon.SetActive(false);
        noWeaponOut = false;
    }

    IEnumerator SwitchToSecondary()
    {
        GetComponent<Animator>().SetTrigger("SwitchWeapon");
        currentWeapon = secondaryWeapon;
        yield return new WaitForSeconds(.66f);
        primaryWeapon.SetActive(false);
        secondaryWeapon.SetActive(true);
        noWeaponOut = false;
    }

    IEnumerator SwitchToNothing()
    {
        GetComponent<Animator>().SetTrigger("SwitchWeapon");
        currentWeapon = secondaryWeapon; //currentWeapon is only used to determine if you can scope or not
        yield return new WaitForSeconds(.66f);
        primaryWeapon.SetActive(false);
        secondaryWeapon.SetActive(false);
        noWeaponOut = true;
    }

    private bool CanSwitch()
    {
        if (Time.time > nextSwitch)
        {
            nextSwitch = Time.time + switchingDelay;
            return true;
        }
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scope.isScoped)
            canSwitchWeapons = false;
        else
            canSwitchWeapons = true;

        Debug.Log("weapon index: " + weapon);

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (!CanSwitch())
                return;

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                weapon++;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                weapon--;

            if (weapon == -1)
                weapon = 2;
            if (weapon == 3)
                weapon = 0;

            switch (weapon)
            {
                case 0:
                    StartCoroutine(SwitchToNothing());
                    break;
                case 1:
                    StartCoroutine(SwitchToPrimary());
                    break;
                case 2:
                    StartCoroutine(SwitchToSecondary());
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && canSwitchWeapons == true)
        {
            if (!CanSwitch())
                return;
            if (primaryWeapon.activeSelf == false )
            {
                //primaryWeapon.SetActive(true);
                //secondaryWeapon.SetActive(false);
                StartCoroutine(SwitchToPrimary());
                // is suspicious
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canSwitchWeapons == true)
        {
            if (!CanSwitch())
                return;
            if (secondaryWeapon.activeSelf == false)
            {
                //primaryWeapon.SetActive(false);
                //secondaryWeapon.SetActive(true);
                StartCoroutine(SwitchToSecondary());
                // is suspicious
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canSwitchWeapons == true)
        {
            if (!CanSwitch())
                return;
            if (noWeaponOut == false)
            {
                //primaryWeapon.SetActive(false);
                //secondaryWeapon.SetActive(false);
                StartCoroutine(SwitchToNothing());
                //is safe
            }
        }
    }
}
