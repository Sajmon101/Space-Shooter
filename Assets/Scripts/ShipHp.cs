using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHp : MonoBehaviour, IHealth
{
    private Slider slider;
    public float maxHp = 2;
    public float hp;
    private TigerShipAttack tigerShipAttack;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        tigerShipAttack = GetComponent<TigerShipAttack>();
        slider.value = 1;
        hp = maxHp;
    }
    public void ReduceHp(float hp)
    {
        this.hp -= hp;
        slider.value -= hp/maxHp;

        if (this.hp<0) Dead();
        tigerShipAttack.isChasing = true;
    }

    private void Dead()
    {
        tigerShipAttack.ShipDead();
    }
}
