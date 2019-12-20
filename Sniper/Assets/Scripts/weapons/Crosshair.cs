using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    public Texture2D[] crosshairTexture;
    public float crosshairScale = 0.2f;
    public float gap;

    void OnGUI()
    {
        //if not paused
        if (Time.timeScale != 0)
        {
            if (crosshairTexture != null)
            {
                GUI.DrawTexture(new Rect(((Screen.width - crosshairTexture[0].width * crosshairScale) / 2) + gap,
                                          ((Screen.height - crosshairTexture[0].height * crosshairScale) / 2),
                                          crosshairTexture[0].width * crosshairScale,
                                          crosshairTexture[0].height * crosshairScale),
                                          crosshairTexture[0]);
                GUI.DrawTexture(new Rect(((Screen.width - crosshairTexture[0].width * crosshairScale) / 2) - gap,
                          ((Screen.height - crosshairTexture[0].height * crosshairScale) / 2),
                          crosshairTexture[0].width * crosshairScale,
                          crosshairTexture[0].height * crosshairScale),
                          crosshairTexture[0]);
                GUI.DrawTexture(new Rect(((Screen.width - crosshairTexture[1].width * crosshairScale) / 2),
                          ((Screen.height - crosshairTexture[1].height * crosshairScale) / 2) + gap,
                          crosshairTexture[1].width * crosshairScale,
                          crosshairTexture[1].height * crosshairScale),
                          crosshairTexture[1]);
                GUI.DrawTexture(new Rect(((Screen.width - crosshairTexture[1].width * crosshairScale) / 2),
                          ((Screen.height - crosshairTexture[1].height * crosshairScale) / 2) - gap,
                          crosshairTexture[1].width * crosshairScale,
                          crosshairTexture[1].height * crosshairScale),
                          crosshairTexture[1]);

            }

        }
    }
}