using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gun : Weapon
{
    public float fireRate = 1.0f;
    public int clipCapacity = 10;
    public int clipsize = 10;

    public GameObject gunHUD;
    public GameObject bulletPrefab;
    public AudioClip shootSound;
    public AudioClip emptyClipSound;

    private Transform bulletSpawner;
    private ParticleSystem muzzleFlash;
    [System.NonSerialized] public int bulletsInClip = 0;
    [System.NonSerialized] public TMPro.TextMeshProUGUI ammoText;
    public LayerMask layerMask;
    private void Awake()
    {
        bulletSpawner = this.transform.Find("Bullet Spawner");
       
        bulletsInClip = clipCapacity;

        if (gunHUD)
        {
            ammoText = gunHUD.GetComponent<TMPro.TextMeshProUGUI>();
            ammoText.text = bulletsInClip.ToString("D3");
        }
    }

    public bool Shoot()
    {
        if (!bulletSpawner)
        {
            return false; 
        }
        if (!bulletPrefab)
        {
            return false;
        }

        if (bulletsInClip > 0)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, float.MaxValue, layerMask))
            {
                GameObject bulletGameObject = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                Bullet bullet = bulletGameObject.GetComponent<Bullet>();
                bullet.OnSpawn(hit);
           
                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }

                bulletsInClip--;

                ammoText.text = bulletsInClip.ToString("D3");

                return true;
            }

            return false;
        }
        else
        {
            if(emptyClipSound != null)
            {
                audioSource.PlayOneShot(emptyClipSound);
            }
            
            return false;
        }
    }

    public void RefillAmmo(int amount)
    {
        if (bulletsInClip + amount >= clipCapacity)
        {
            bulletsInClip = clipCapacity;
        }
        else
        {
            bulletsInClip = bulletsInClip + amount;
        }

        ammoText.text = bulletsInClip.ToString("D3");
    }

    override public bool UseWeapon() 
    {
        return Shoot();
    }
}
