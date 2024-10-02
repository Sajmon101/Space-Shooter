using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Weapon> weapons;
    public int numberOfKeys = 0;
    public int indexOfSlotInUse = 0;
    public float weaponChangeCooldown = 1.0f;

    private float nextChangeTime;
    private Attack attack;
    [System.NonSerialized] public GameObject weaponSlot;

    public void Start()
    {
        attack = GetComponent<Attack>();
        weaponSlot = GameObject.Find("First Person Controller/First Person Camera/Weapon Slot");

        if (weaponSlot)
        {
            if (weapons.Count > 0)
            {
                if(attack)
                {
                    for (int i = 0; i < weapons.Count; ++i)
                    {
                        weapons[i] = Instantiate(weapons[i].gameObject, weaponSlot.transform.position, weaponSlot.transform.rotation).GetComponent<Weapon>();

                        weapons[i].gameObject.transform.parent = weaponSlot.transform;

                        if (i > 0)
                        {
                            weapons[i].gameObject.SetActive(false);
                        }
                    }
                    
                    attack.weapon = weapons[0];
                }             
            }
        }
    }

    public void Update()
    {
        if (weapons.Count >= 2)
        {
            if (Time.time >= nextChangeTime)
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        int targetIndex = (i == 0) ? 9 : i - 1;

                        if (weapons.Count > targetIndex && targetIndex >= 0 && targetIndex != indexOfSlotInUse)
                        {
                            indexOfSlotInUse = targetIndex;
                            ChangeWeaponSlot(indexOfSlotInUse);
                            nextChangeTime = Time.time + weaponChangeCooldown;
                        }

                        break;
                    }
                }
            }
        }
    }

    public void ChangeWeaponSlot(int weaponIndex)
    {
        if (attack)
        {
            if (weaponSlot)
            {
                Weapon weapon = weapons[weaponIndex];
                if (weapon)
                {
                    foreach (Transform child in weaponSlot.transform)
                    {
                        child.gameObject.SetActive(false);
                    }

                    attack.weaponModel = weapon.gameObject;
                    attack.weapon = weapon;

                    attack.weapon.gameObject.SetActive(true);
                }
            }
        }
    }
}
