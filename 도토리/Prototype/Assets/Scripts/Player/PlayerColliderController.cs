using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    PlayerController player;

    public CapsuleCollider2D normalCol;
    public CapsuleCollider2D dieCol;

    public CapsuleCollider2D normalWeaponCol;
    public CapsuleCollider2D fireWeaponCol;

    public WeaponSword weaponSwordCollider;

    private void Awake()
    {
        player = transform.GetComponent<PlayerController>();
    }

    public void ChangeDieCol()
    {
        if(GameManager.Instance.isPlayerDead)
        {
            //player.playerCollider = dieCol;
            player.playerCollider.size = dieCol.size;
            player.playerCollider.offset = dieCol.offset;
        }
        else
        {
            player.playerCollider.size = normalCol.size;
            player.playerCollider.offset = normalCol.offset;
        }
    }

    public void ChangeWeaponCol()
    {
        if(player.stat.curWeaponProperty==ElementalProperty.Fire)
        {
            weaponSwordCollider.weaponCol.size = fireWeaponCol.size;
            weaponSwordCollider.weaponCol.offset = fireWeaponCol.offset;
        }
        else
        {
            weaponSwordCollider.weaponCol.size = normalWeaponCol.size;
            weaponSwordCollider.weaponCol.offset = normalWeaponCol.offset;
        }
    }

}
