using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassItem : MonoBehaviour
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

                if (player.GetComponent<PlayerController>().stat.curWeaponProperty != ElementalProperty.Grass)
                {
                    GameManager.Instance.isItemEatting = true;
                player.GetComponent<PlayerController>().stat.curWeaponProperty = ElementalProperty.Grass;

                    EffectManager.Instance.AttackEffectChange(player.GetComponent<PlayerController>().stat.curWeaponProperty);

                    player.GetComponent<PlayerController>().anim.SetInteger("curProperty", (int)ElementalProperty.Grass);
                }

                EffectManager.Instance.SetElementalEffect((int)ElementalProperty.Grass - 1);
                Destroy(gameObject);
            }
        }
    }
}
