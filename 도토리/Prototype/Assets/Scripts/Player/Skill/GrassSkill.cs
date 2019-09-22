using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSkill : IPassiveSkill
{
    private const float GRASS_DEMN = 0.5f;

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
        return GRASS_DEMN;
    }

}
