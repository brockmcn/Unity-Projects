using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {

    public void Death()
    {
        GameObject.Instantiate((GameObject)Resources.Load("Ragdoll"), gameObject.transform.parent.position, gameObject.transform.parent.rotation);
        Destroy(gameObject);
    }

}
