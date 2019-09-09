using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Range(0f, 10f)]
    public float moveSpeed;
    [Range(0f, 20f)]
    public float fallSpeed;

    public float attackRange;
    public float attackRate;

    public float currentHp;
    public float hp;

    public CharacterStat lastHitBy = null;

    public IFSMManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<IFSMManager>();
        currentHp = hp;
    }

    public void NotifyDead()
    {
        manager.NotifyTargetDead();
    }

    public void ApplyDamage(CharacterStat from)
    {
        currentHp -= from.attackRate;
        if (currentHp <= 0)
        {
            if (lastHitBy == null)
            {
                lastHitBy = from;

            }
            manager.SetDead();
            from.NotifyDead();
        }
        
    }
}
