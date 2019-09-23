using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill : IPassiveSkill
{
    private const float WATER_DEMN = 0.7f;

    public float DamageMUL(float x, float y)
    {
        return x * y;
    }

    public float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        return calc(weaponDamage, elementalDamage);
    }

    public float GetBuff()
    {
        return WATER_DEMN;
    }
}
