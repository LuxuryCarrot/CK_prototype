using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float DamageCalc(float a,float b);
public interface IPassiveSkill
{
    float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc);
    float DamageMUL(float x, float y);
    float GetBuff();
}