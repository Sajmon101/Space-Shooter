using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoRefillAmount = 10;
    public bool shouldRespawnAmmoBox = true;
    public float ammoBoxRespawnDelay = 5.0f;

    public GameObject AmmoBoxModel;

    private void OnTriggerEnter(Collider other)
    {
        if(!AmmoBoxModel.activeSelf)
        {
            return; 
        }

        Attack attack = other.gameObject.GetComponent<Attack>();
        if (other.gameObject.tag == "Player" && attack != null && attack.weapon != null)
        {   
            Gun gun = attack.weapon as Gun;
            if (gun.bulletsInClip < gun.clipCapacity)
            {
                gun.RefillAmmo(ammoRefillAmount);

                AmmoBoxModel?.gameObject.SetActive(false);
               
                if (shouldRespawnAmmoBox)
                {
                    Invoke("RespawnAmmoBoxModel", ammoBoxRespawnDelay);
                }
            }
        }
    }

    private void RespawnAmmoBoxModel()
    {
        AmmoBoxModel?.gameObject.SetActive(true);
    }
}
