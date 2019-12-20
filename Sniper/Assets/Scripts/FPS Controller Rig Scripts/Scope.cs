using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour {

    private GameObject currentWeapon;

    public Animator animator;

    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    private Crosshair crossHair;
    public Camera fpsCamera;

    public float scopedFOV = 15f;
    private float normalFOV;
    

    public bool isScoped = false;

    private ScopeBob2 bobbing;
    private Vector3 originalPosition;


    private void Start()
    {
        crossHair = FindObjectOfType<Crosshair>().GetComponent<Crosshair>();
        bobbing = GetComponentInParent<ScopeBob2>();
        
    }

    private void Update()
    {
        currentWeapon = transform.GetComponent<SwitchWeapon>().currentWeapon;
        if (Input.GetButtonDown("Fire2") && currentWeapon.GetComponent<gunScript>().isSniper)
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.18f);//delay before scope overlay replaces weapon after begin scoped animation starts, .15f to align exactly

        scopeOverlay.SetActive(true);
        crossHair.enabled = false;
        weaponCamera.SetActive(false);

        bobbing.isZoomed = true;

        normalFOV = fpsCamera.fieldOfView;
        fpsCamera.fieldOfView = scopedFOV;

    }

    void OnUnScoped()
    {
        scopeOverlay.SetActive(false);
        crossHair.enabled = true;
        weaponCamera.SetActive(true);

        bobbing.isZoomed = false;

        fpsCamera.fieldOfView = normalFOV;
    }
}
