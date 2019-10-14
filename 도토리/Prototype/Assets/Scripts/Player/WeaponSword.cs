using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponSword : MonoBehaviour
{
    public PlayerStats stat;

    private PlayerController player;
    private CapsuleCollider2D weaponCol;
    private BoxCollider2D monsterCol;

    [HideInInspector]
    public Transform monster;

    bool isMonsterCheck;

    DamageCalc dele;

    IPassiveSkill firePassive = new FireSkill();
    IPassiveSkill waterPassive = new WaterSkill();
    IPassiveSkill grassPassive = new GrassSkill();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        weaponCol = GetComponent<CapsuleCollider2D>();
        stat = player.transform.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (isMonsterCheck)
        {
            if (monster != null)
            {
                if (player.curAttackAnimSpeed >= PlayerController.MAX_ATTACK_ANIM_TIME)
                {

                    var animal = monster.GetComponent<Monster>().animalname;

                    stat.finalDamage = DamageCalc(ref animal);


                    //대미지 텍스트를 몬스터 위치에 생성
                    var cloneText = Instantiate(GameManager.Instance.ui.prefab_floating_text,
                    Camera.main.WorldToScreenPoint(new Vector3(monster.position.x, monster.position.y + 1f, monster.position.z)), Quaternion.identity);

                    cloneText.GetComponent<FloatingDamageText>().text.text = Convert.ToString(stat.finalDamage);
                    cloneText.transform.SetParent(GameManager.Instance.ui.canvas.transform);
                    ////////////////////////////////////////////////////////////////////////////////////


                    Debug.Log("Player Damage : " + stat.finalDamage);
                    monster.SendMessage("ApplyDamage", stat.finalDamage);
                    isMonsterCheck = false;
                }
            }
        }
    }


    public float DamageCalc(ref AnimalName animal)
    {
        float damage = 0;
        float fireDamage = firePassive.GetDamage(stat.weaponDamage);

        switch (animal)
        {
            case AnimalName.FOX:
                if (player.stat.curWeaponProperty == ElementalProperty.Water)
                {
                    dele = new DamageCalc(waterPassive.DamagePercent);
                    damage = waterPassive.EmpowerInWeapon(stat.weaponDamage, 50f, dele);
                }
                else if (player.stat.curWeaponProperty == ElementalProperty.Fire)
                {
                    damage = fireDamage;
                }
                else
                    damage = stat.weaponDamage;
                break;
            case AnimalName.FROG:
                if (player.stat.curWeaponProperty == ElementalProperty.Grass)
                {
                    dele = new DamageCalc(grassPassive.DamagePercent);
                    damage = grassPassive.EmpowerInWeapon(stat.weaponDamage, 50f, dele);
                }
                else if (player.stat.curWeaponProperty == ElementalProperty.Fire)
                {
                    damage = fireDamage;
                }
                else
                    damage = stat.weaponDamage;
                break;
            case AnimalName.RACCOON:
                if (player.stat.curWeaponProperty == ElementalProperty.Fire)
                {
                    dele = new DamageCalc(firePassive.DamagePercent);
                    damage = firePassive.EmpowerInWeapon(fireDamage, 50f, dele);
                }
                else
                {
                    damage = stat.weaponDamage;
                }
                break;
        }

        return damage;
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
