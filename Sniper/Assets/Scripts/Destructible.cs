using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public void recieveDamage() //written wrong but thats what it says in the projectilescript
    {
        gameObject.SetActive(false);
    }
}
