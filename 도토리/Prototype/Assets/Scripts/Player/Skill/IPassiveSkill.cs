using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float DamageCalc(float a);
public interface IPassiveSkill
{
    float Empower(float weaponDamage, float elementalDamage, DamageCalc calc);
    float GetBuff();
    float DamageMUL(float weaponDamage);
}