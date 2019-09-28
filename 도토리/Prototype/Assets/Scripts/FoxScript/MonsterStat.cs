using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public float moveSpeed;
    public float attackRange;
    public float attackRate;

    public float currentHp;
    public float Hp;

    public MonsterIFSM manager;

    private void Awake()
    {
        manager = GetComponent<MonsterIFSM>();

        currentHp = Hp;

    }

    public void ApplyDamage(MonsterStat from)
    {
        currentHp -= from.attackRate;
        if (currentHp <= 0)
        {
            manager.SetDead();
        }
    }
}
