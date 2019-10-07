using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : IPassiveSkill
{
    private const float FIRE_DEMN = 15f;

    public float DamageMUL(float x, float y)
    {
        return x + y;
    }

    public float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        return calc(weaponDamage, elementalDamage);
    }

    public float GetBuff()
    {
        return FIRE_DEMN;
    }
}
