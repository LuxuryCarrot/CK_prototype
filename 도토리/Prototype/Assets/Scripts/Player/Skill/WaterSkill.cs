using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill : IPassiveSkill
{
    private const float WATER_DEMN = 0.7f;

    public float DamageMUL(float weaponDamage)
    {
        return weaponDamage * this.GetBuff();
    }

    public float Empower(float weaponDamage, float elementalDamage, DamageCalc calc)
    {
        Debug.Log("무기 공격력 : " + weaponDamage);
        Debug.Log("속성 공격력 : " + elementalDamage);
        return calc(weaponDamage);
    }

    public float GetBuff()
    {
        return WATER_DEMN;
    }
}
