using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


public class PlayerATTACK : PlayerFSMController
{
    DamageCalc dele;

    IPassiveSkill firePassive = new FireSkill();
    IPassiveSkill waterPassive = new WaterSkill();
    IPassiveSkill grassPassive = new GrassSkill();


    public override void BeginState()
    {
        base.BeginState();

        controller.curAttackAnimSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (controller.curWeaponProperty)
        {
            case ElementalProperty.Fire:
                dele = new DamageCalc(firePassive.DamageMUL);
                controller.stat.finalDamage = firePassive.EmpowerInWeapon(controller.stat.weaponDamage, firePassive.GetBuff(), dele);
                break;
            case ElementalProperty.Water:
                dele = new DamageCalc(waterPassive.DamageMUL);
                controller.stat.finalDamage = waterPassive.EmpowerInWeapon(controller.stat.weaponDamage, waterPassive.GetBuff(), dele);
                break;
            case ElementalProperty.Grass:
                dele = new DamageCalc(grassPassive.DamageMUL);
                controller.stat.finalDamage = grassPassive.EmpowerInWeapon(controller.stat.weaponDamage, grassPassive.GetBuff(), dele);
                break;
            default:                                   //None
                controller.stat.finalDamage = controller.stat.weaponDamage;
                break;
        }

        if (controller.curAttackAnimSpeed >= PlayerController.MAX_ATTACK_ANIM_TIME)
        {
            controller.curAttackAnimSpeed = 0;
            controller.AttackDirCheck(0);
            controller.states[PlayerState.ATTACK].enabled = false;
        }
        else
        {
            controller.curAttackAnimSpeed += Time.deltaTime;
        }
    }
}

