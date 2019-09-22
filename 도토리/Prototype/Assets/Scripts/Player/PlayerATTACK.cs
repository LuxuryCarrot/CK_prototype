using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


public class PlayerATTACK : PlayerFSMController
{
    DamageCalc calc;
    float finalDamage;

    IPassiveSkill firePassive = new FireSkill();
    IPassiveSkill waterPassive = new WaterSkill();
    IPassiveSkill grassPassive = new GrassSkill();

    public override void BeginState()
    {
        base.BeginState();
    }

    // Update is called once per frame
    void Update()
    {
        switch (controller.curWeaponProperty)
        {
            case ElementalProperty.Fire:
                calc = new DamageCalc(firePassive.DamageMUL);
                finalDamage = firePassive.Empower(controller.weaponDamage, firePassive.GetBuff(), calc);
                break;
            case ElementalProperty.Water:
                calc = new DamageCalc(waterPassive.DamageMUL);
                finalDamage = waterPassive.Empower(controller.weaponDamage, waterPassive.GetBuff(), calc);
                break;
            case ElementalProperty.Grass:
                calc = new DamageCalc(grassPassive.DamageMUL);
                finalDamage = grassPassive.Empower(controller.weaponDamage, grassPassive.GetBuff(), calc);
                break;
            default:                        //None
                finalDamage = controller.weaponDamage;
                break;
        }

        Debug.Log(finalDamage);


        controller.states[PlayerState.ATTACK].enabled = false;
    }
}

