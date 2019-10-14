using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterItem : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (player.GetComponent<PlayerController>().stat.curWeaponProperty != ElementalProperty.Water)
                {
                    GameManager.Instance.isItemEatting = true;
                player.GetComponent<PlayerController>().stat.curWeaponProperty = ElementalProperty.Water;

                    EffectManager.Instance.AttackEffectChange(player.GetComponent<PlayerController>().stat.curWeaponProperty);

                    player.GetComponent<PlayerController>().anim.SetInteger("curProperty", (int)ElementalProperty.Water);
                }
                EffectManager.Instance.SetElementalEffect((int)ElementalProperty.Water - 1);
                Destroy(gameObject);
            }
        }
    }
}
