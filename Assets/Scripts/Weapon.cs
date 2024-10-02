using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int usesPerSecond = 1;
    public int weaponID = 0;

    [System.NonSerialized] public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual bool UseWeapon() { return false; }
}
