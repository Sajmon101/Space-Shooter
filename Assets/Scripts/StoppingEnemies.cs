using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingEnemies : MonoBehaviour
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
                EnemyBrain[] enemiesScripts = enemies.GetComponentsInChildren<EnemyBrain>();

                foreach (EnemyBrain enemyScript in enemiesScripts)
                {
                    enemyScript.StopRobot();
                }
            }
        }
    }
}
