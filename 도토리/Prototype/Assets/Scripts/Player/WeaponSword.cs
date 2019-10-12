using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : MonoBehaviour
{
    public PlayerStats stat;

    private PlayerController player;
    private CapsuleCollider2D weaponCol;
    private BoxCollider2D monsterCol;

    [HideInInspector]
    public Transform monster;

    bool isMonsterCheck;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        weaponCol = GetComponent<CapsuleCollider2D>();
        stat = player.transform.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if(isMonsterCheck)
        {
            if (monster != null)
            {
                if (player.curAttackAnimSpeed >= PlayerController.MAX_ATTACK_ANIM_TIME)
                {
                    Debug.Log("Player Damage : " + stat.finalDamage);
                    monster.SendMessage("ApplyDamage", stat.finalDamage);
                    isMonsterCheck = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag.CompareTo("Monster") == 0) &&
            (stat.controller.states[PlayerState.ATTACK].enabled/*stat.curState == PlayerState.ATTACK*/))
        {
            monster = collision.transform;
            monsterCol = monster.GetComponent<BoxCollider2D>();

            if (collision == monsterCol)
            {
                isMonsterCheck = true;
            }
        }
    }
}
