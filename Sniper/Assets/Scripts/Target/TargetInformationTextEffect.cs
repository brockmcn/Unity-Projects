using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetInformationTextEffect : MonoBehaviour {

    public string text;
    public Text textObject;
    public float delay = .1f;

    private void Start()
    {
        textObject.text = "";
        StartCoroutine(EffectThread());
    }

    IEnumerator EffectThread()
    {
        textObject.text += "_";
        yield return new WaitForSeconds(.1f);
        for(int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = textObject.text.Remove(textObject.text.Length - 1);
            textObject.text += text[i];
            textObject.text += "_";
        }
        textObject.text = textObject.text.Remove(textObject.text.Length - 1);
    }
}
