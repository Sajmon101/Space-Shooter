using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLight : MonoBehaviour
{
    private bool blinkingDone = false;
    public Light directionalLight;
    public float blinkInterval;
    public int blinkCount;


    private void OnTriggerEnter(Collider other)
    {
        if (!blinkingDone)
        {
            StartCoroutine(BlinkLight());
            blinkingDone = true;
        }
    }

    private IEnumerator BlinkLight()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // Turn the light on
            directionalLight.enabled = true;

            // Wait for the specified interval
            yield return new WaitForSeconds(blinkInterval);

            // Turn the light off
            directionalLight.enabled = false;

            // Wait for the same interval
            yield return new WaitForSeconds(blinkInterval);
        }

        // At the end, turn off the light
        directionalLight.enabled = false;
    }
}
