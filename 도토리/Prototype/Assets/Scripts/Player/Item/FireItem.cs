using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : MonoBehaviour
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

                if (player.GetComponent<PlayerController>().stat.curWeaponProperty != ElementalProperty.Fire)
                {
                    GameManager.Instance.isItemEatting = true;              //처음 먹었을 때만 
                player.GetComponent<PlayerController>().stat.curWeaponProperty = ElementalProperty.Fire;

                    player.GetComponent<PlayerColliderController>().ChangeWeaponCol();

                    EffectManager.Instance.AttackEffectChange(player.GetComponent<PlayerController>().stat.curWeaponProperty);

                    player.GetComponent<PlayerController>().anim.SetInteger("curProperty", (int)ElementalProperty.Fire);
                }
                EffectManager.Instance.SetElementalEffect((int)ElementalProperty.Fire - 1);
                Destroy(gameObject);
            }
        }
    }
}
