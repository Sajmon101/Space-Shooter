using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoors : MonoBehaviour
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
    public Vector3 leftDoorOrigin;
    public Vector3 rightDoorOrigin;
    public bool doorOpen = false;
    public bool doorOpening = false;

    public void Start()
    {
        leftDoorOrigin = LeftDoor.transform.position;
        rightDoorOrigin = RightDoor.transform.position;
    }
    public void OnTriggerEnter(Collider other)
    {
        Inventory equipment = other.GetComponent<Inventory>();
        if (equipment != null)
        {
            if (equipment.numberOfKeys > 0)
            {
                equipment.numberOfKeys--;
                doorOpening = true;
            }
        }
    }

    public void Update()
    {
        if (!doorOpen && doorOpening)
        {
            LeftDoor.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
            RightDoor.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
            GetComponent<BoxCollider>().enabled = false;

            if (LeftDoor.transform.position == leftDoorOrigin + new Vector3(5, 0, 0))
            {
                doorOpening = false;              
            }
        }
    }
}
