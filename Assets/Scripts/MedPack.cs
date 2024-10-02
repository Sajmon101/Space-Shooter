using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedPack : MonoBehaviour
{
    public float healtPointsRefillAmount = 10.0f;
    public bool shouldRespawnMedPack = true;
    public float medPackRespawnDelay = 5.0f;

    private GameObject MedPackModel;

    private void Start()
    {
        MedPackModel = GameObject.Find("MedPackModel");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!MedPackModel.activeSelf)
        {
            return;
        }

        Health health = collision.gameObject.GetComponent<Health>();
        if (collision.gameObject.tag == "Player" && health != null)
        {
            if (health.currentHealth < health.maxHealth)
            {
                health.Heal(healtPointsRefillAmount);
                MedPackModel?.SetActive(false);

                if (shouldRespawnMedPack)
                {
                    Invoke("RespawnMedPackModel", medPackRespawnDelay);
                }
            }
        }
    }

    private void RespawnMedPackModel()
    {
        MedPackModel?.SetActive(true);
    }
}
