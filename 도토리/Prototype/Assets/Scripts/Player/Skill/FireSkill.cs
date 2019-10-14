using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : IPassiveSkill
{
    private const float BASE_FIRE_DEMN = 15f;
    private float finalFireDemn = 0;

    public float DamagePercent(float x, float y)       
    {
        return x += ((x * y) / 100f);        
    }


    public float EmpowerInWeapon(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        return calc(weaponDamage, elementalDamage);
    }

    public float GetDamage(float x = 0)
    {
        return x + BASE_FIRE_DEMN;
    }
}
