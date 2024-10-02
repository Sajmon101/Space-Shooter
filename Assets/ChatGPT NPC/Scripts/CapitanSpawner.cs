using GDL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CapitanSpawner : MonoBehaviour
{
    [SerializeField] private GameObject capitan;
    [SerializeField] private Transform enemyContainer;

    private void Update()
    {
        if(enemyContainer.childCount == 0)
        {
            capitan.SetActive(true);
            enabled = false;
        }
        
        if(Input.GetKeyUp(KeyCode.F12))
        {
            int size = enemyContainer.childCount;

            for (int i = 0; i < size; i++)
            {
                Destroy(enemyContainer.GetChild(i).gameObject);
            }
        }
    }
}
