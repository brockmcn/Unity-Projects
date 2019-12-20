using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour {

    private bool isPlaying = false;
    public List<AudioSource> audioSourcesToSilence = new List<AudioSource>();

    void Start () {
        gameObject.GetComponent<AudioSource>().Stop();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.P) && !isPlaying)
        {
            isPlaying = true;
            gameObject.GetComponent<AudioSource>().Play();
            foreach (AudioSource audioSourcesToSilence in audioSourcesToSilence)
                audioSourcesToSilence.volume = 0.05f;
        }

        else if (Input.GetKeyDown(KeyCode.P) && isPlaying)
        {
            isPlaying = false;
            gameObject.GetComponent<AudioSource>().Pause();
            foreach (AudioSource audioSourcesToSilence in audioSourcesToSilence)
                audioSourcesToSilence.volume = 1;
        }
    }
}
