using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SneakyHp : MonoBehaviour, IHealth
{
    private Slider slider;
    public float maxHp = 2;
    public float hp;
    private SneakyShipAttack sneakyShipAttack;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        sneakyShipAttack = GetComponent<SneakyShipAttack>();
        slider.value = 1;
        hp = maxHp;
    }
    public void ReduceHp (float hp)
    {
        this.hp -= hp;
        slider.value -= hp/maxHp;

        if (this.hp<0) Dead();
        sneakyShipAttack.isChasing = true;
    }

    private void Dead()
    {
        sneakyShipAttack.ShipDead();
    }
}