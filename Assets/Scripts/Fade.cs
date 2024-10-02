using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float TimeToFade = 1.0f;
    private bool fadeOut = false; 
    void Start()
    {
        
    }
    void Update()
    {
        if(fadeOut) 
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= TimeToFade * Time.deltaTime;
                if (canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    public void FadeOut() 
    {
        canvasGroup.alpha = 1;
        fadeOut = true;
    }
}
