//Make sure this is on the bullet
//The only thing that should be modified is selfDestruct, as it determines how long the bullet will remain in the scene

using UnityEngine;

public class bulletScript : MonoBehaviour {

    public float damage;
    public float selfDestruct;
    private float time;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
