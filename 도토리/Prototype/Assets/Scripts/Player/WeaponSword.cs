using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer weaponSprite;
    private CapsuleCollider2D weaponCol;
    private Vector3 weaponHandPos;
    private Vector2 spriteRightOffset;
    private Vector2 spriteLeftOffset;

    private Vector3 mousePos;

    private bool isWeaponFirstPosChanged = false;


    // Start is called before the first frame update
    void Start()
    {
        player = transform.root;
        weaponSprite = GetComponent<SpriteRenderer>();
        weaponCol = GetComponent<CapsuleCollider2D>();
        weaponHandPos = transform.parent.localPosition;
        spriteRightOffset = weaponCol.offset;
        spriteLeftOffset = -weaponCol.offset;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        if (player.position.x > mousePos.x)
        {
            transform.parent.localPosition =
                new Vector3(-weaponHandPos.x, transform.parent.localPosition.y, transform.parent.localPosition.z);

            weaponCol.offset = spriteLeftOffset;
            weaponSprite.flipY = true;
            weaponSprite.flipX = true;
        }
        else
        {
            transform.parent.localPosition =
                new Vector3(weaponHandPos.x, weaponHandPos.y, weaponHandPos.z);

            weaponCol.offset = spriteRightOffset;
            weaponSprite.flipY = false;
            weaponSprite.flipX = false;
        }

    }
}
