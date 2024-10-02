using System.Diagnostics;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [System.NonSerialized] public float nextFireTime;
    [System.NonSerialized] public GameObject weaponModel;
    [System.NonSerialized] public Weapon weapon;
    private Inventory inventory;
    private WeaponMovementController weaponMovementController;

    private void Start()
    {
        weaponMovementController = GetComponentInChildren<WeaponMovementController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time >= nextFireTime)
            {
                float usesPerSecond = UseWeapon();
                nextFireTime = Time.time + 1f / usesPerSecond;
            }
        }
        weaponMovementController?.HandleRecoil(false);
    }

    float UseWeapon()
    {
        if (weapon.UseWeapon())
        {
            weaponMovementController?.HandleRecoil(true);
            return weapon.usesPerSecond;
        }
        else
        {
            return 0.5f;
        }
    }
}