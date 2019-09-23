using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSkill : IPassiveSkill
{
    private const float GRASS_DEMN = 0.4f;

    public float DamageMUL(float x, float y)
    {
        return x * y;
    }

    public float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        return calc(weaponDamage,elementalDamage);
    }

    public float GetBuff()
    {
        return GRASS_DEMN;
    }

}
