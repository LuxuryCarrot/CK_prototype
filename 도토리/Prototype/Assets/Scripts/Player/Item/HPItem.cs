using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : MonoBehaviour
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
                GameManager.Instance.ui.playerHP.fillAmount += GameManager.Instance.damageToPlayerHp;
                Destroy(gameObject);
            }
        }
    }
}
