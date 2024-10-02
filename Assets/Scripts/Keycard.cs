using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Inventory equipment = other.GetComponent<Inventory>();
        if (equipment != null)
        {
            equipment.numberOfKeys++;
            
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
