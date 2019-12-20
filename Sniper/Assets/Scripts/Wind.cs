using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public float xWind;
    public float yWind;
    public float zWind;
    public projectile2 projectile2;

	void Start () {
        RandomWind();
	}
	
	void Update () {
        projectile2.windSpeed = new Vector3(xWind, yWind, zWind);     
	}

    void RandomWind()
    {
        xWind = Random.Range(-0.5f, 0.5f);
        yWind = Random.Range(-0.005f, 0.005f);
        zWind = Random.Range(-0.01f, 0.01f);
        StartCoroutine(Interval());
    }

    public IEnumerator Interval()
    {
        yield return new WaitForSeconds(10);
        RandomWind();
    }
}
