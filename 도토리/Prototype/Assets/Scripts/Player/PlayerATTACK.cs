﻿using System.Collections;
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

    const float maxAttackAnimTime = 0.3f;
    float curAnimSpeed;

    public override void BeginState()
    {
        base.BeginState();

        if (controller.attackDir < 1)                               //Prev Anim down attack
            controller.AttackDirCheck(controller.isUpAttacked);
        else                                                        // Prev Anim up attack 
            controller.attackDir = 0;
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
        if (curAnimSpeed >= maxAttackAnimTime)
        {
            curAnimSpeed = 0;
            controller.states[PlayerState.ATTACK].enabled = false;
        }
        else
        {
            curAnimSpeed += Time.deltaTime;
        }
    }
}

