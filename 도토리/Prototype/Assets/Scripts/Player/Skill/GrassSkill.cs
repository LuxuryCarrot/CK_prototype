using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSkill : IPassiveSkill
{
    private const float BASE_GRASS_DEMN = 5f;
    private float finalGrassDemn;


    public float DamagePercent(float x, float y)
    {
        return x += ((x * y) / 100f);
    }

    public float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        return calc(weaponDamage,elementalDamage);
    }

    public float GetDamage(float x)
    {
        return x+ BASE_GRASS_DEMN;
    }

}
