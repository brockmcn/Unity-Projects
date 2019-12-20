using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCallForBackUp : MonoBehaviour {

    PoliceShoot shoot;

    private void Awake()
    {
        shoot = GetComponent<PoliceShoot>();
    }

    private void OnEnable()
    {
        SpawnPoliceNearBy();
    }

    void SpawnPoliceNearBy()
    {
        //for loop spawning however many police or you can also use invoke repeating if you want doesnt matter too much 
        //when finished go back to shooting
        shoot.enabled = true;
        shoot.backupCalled = true;
        enabled = false;
    }

    private void OnDisable()
    {
        shoot.backupCalled = true;
    }
}
