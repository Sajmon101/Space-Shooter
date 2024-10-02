using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{

    private AudioSource sound;
    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    public void PlayAudio()
    {
        sound.Play();
        sound.transform.parent = null;
        Destroy(gameObject, 2f);
    }
}
