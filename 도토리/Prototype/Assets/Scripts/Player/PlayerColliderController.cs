using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    PlayerController player;

    Collider2D normalCol;
    Collider2D returnCol;
    Collider2D dieCol;

    Collider2D normalWeaponCol;
    Collider2D returnWeaponCol;
    Collider2D fireWeaponCol;

    private void Awake()
    {
        player = transform.GetComponent<PlayerController>();
    }

    public void ChangeDieCol()
    {
        if(GameManager.Instance.isPlayerDead)
        {
            returnCol = normalCol;
            normalCol = dieCol;
        }
        else
        {
            normalCol = returnCol;
        }

    }

}
