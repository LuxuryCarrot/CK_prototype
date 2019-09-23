using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


public class PlayerATTACK : PlayerFSMController
{
    DamageCalc dele;
    float finalDamage;


    const float startUpSlashAngle = 180f;
    const float maxUpSlashAngle = 360f;

    const float startDownSlashAngle = 360;
    const float maxDownSlashAngle = 180f;

    float slashAngleSpeed = 600f;
    float curSlashAngle = 0;

    IPassiveSkill firePassive = new FireSkill();
    IPassiveSkill waterPassive = new WaterSkill();
    IPassiveSkill grassPassive = new GrassSkill();



    public override void BeginState()
    {
        base.BeginState();
        if (controller.attackDir < 1)                               //Prev Anim down attack
        {
            curSlashAngle = startUpSlashAngle;
            controller.AttackDirCheck(controller.isUpAttacked);
        }
        else                                                        // Prev Anim up attack 
        {
            curSlashAngle = startDownSlashAngle;
            controller.attackDir = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (controller.curWeaponProperty)
        {
            case ElementalProperty.Fire:
                dele = new DamageCalc(firePassive.DamageMUL);
                finalDamage = firePassive.EmpowerInWeapon(controller.weaponDamage, firePassive.GetBuff(), dele);
                break;
            case ElementalProperty.Water:
                dele = new DamageCalc(waterPassive.DamageMUL);
                finalDamage = waterPassive.EmpowerInWeapon(controller.weaponDamage, waterPassive.GetBuff(), dele);
                break;
            case ElementalProperty.Grass:
                dele = new DamageCalc(grassPassive.DamageMUL);
                finalDamage = grassPassive.EmpowerInWeapon(controller.weaponDamage, grassPassive.GetBuff(), dele);
                break;
            default:                                   //None
                finalDamage = controller.weaponDamage;
                break;
        }


        Debug.Log(controller.weapon.transform.rotation);

        //controller.monster.SendMessage("ApplyDamage", finalDamage);

        if (controller.attackDir < 1)                               //Anim down attack
        {
            curSlashAngle -= Time.deltaTime * slashAngleSpeed;
            curSlashAngle = Mathf.Clamp(curSlashAngle, maxDownSlashAngle, startDownSlashAngle);
            controller.weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, curSlashAngle));

            if (curSlashAngle <= maxDownSlashAngle)
            {
                controller.weapon.transform.rotation = controller.startAngle;
                controller.states[PlayerState.ATTACK].enabled = false;
            }
        }
        else                                                        //Anim up attack 
        {
            curSlashAngle += Time.deltaTime * slashAngleSpeed;
            curSlashAngle = Mathf.Clamp(curSlashAngle, startUpSlashAngle, maxUpSlashAngle);
            controller.weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, curSlashAngle));

            if (curSlashAngle >= maxUpSlashAngle)
            {
                controller.weapon.transform.rotation = controller.startAngle;
                controller.states[PlayerState.ATTACK].enabled = false;
            }
        }
    }
}

