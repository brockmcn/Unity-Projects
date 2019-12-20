using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipping : MonoBehaviour {

    public GameObject fadingPlane;
    private bool inAnimation = false;
    public GameObject gameController;
    public GameObject watch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H) && !inAnimation)
        {
            StartCoroutine(FadeWatch());
        }
	}

    IEnumerator FadeOut()
    {
        while (fadingPlane.GetComponent<Image>().color.a < 1)
        {
            yield return new WaitForSeconds(.01f);
            Color newColor = fadingPlane.GetComponent<Image>().color;
            newColor.a += .01f;
            fadingPlane.GetComponent<Image>().color = newColor;
        }
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (fadingPlane.GetComponent<Image>().color.a > 0)
        {
            yield return new WaitForSeconds(.01f);
            Color newColor = fadingPlane.GetComponent<Image>().color;
            newColor.a -= .01f;
            fadingPlane.GetComponent<Image>().color = newColor;
        }
        inAnimation = false;
    }

    IEnumerator FadeWatch()
    {
        inAnimation = true;
        watch.SetActive(true);
        while (watch.transform.localScale.x < .1)
        {
            yield return new WaitForSeconds(.01f);
            Vector3 scale = watch.transform.localScale;
            scale.x += .01f;
            scale.y += .01f;
            scale.z += .01f;
            watch.transform.localScale = scale;
        }
        yield return new WaitForSeconds(1f);
        gameController.GetComponent<GameTime>().time = gameController.GetComponent<GameTime>().time.AddHours(2);
        yield return new WaitForSeconds(1f);
        while (watch.transform.localScale.x > 0)
        {
            yield return new WaitForSeconds(.01f);
            Vector3 scale = watch.transform.localScale;
            scale.x -= .01f;
            scale.y -= .01f;
            scale.z -= .01f;
            watch.transform.localScale = scale;
        }
        yield return new WaitForSeconds(1);
        watch.SetActive(false);
        StartCoroutine(FadeOut());
    }
}
