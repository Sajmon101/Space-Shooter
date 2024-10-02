using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemies : MonoBehaviour
{
    [SerializeField] int roomNr;

    private void OnTriggerEnter(Collider other)
    {
        GameObject room = GameObject.Find("Room" + roomNr.ToString());

        if (room != null)
        {
            GameObject enemies = room.transform.Find("Enemies").gameObject;

            if (enemies != null)
            {
                enemies.gameObject.SetActive(true);
            }
        }
    }
}
