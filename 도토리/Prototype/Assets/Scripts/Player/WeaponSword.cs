using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : MonoBehaviour
{
    public PlayerStats stat;

    private Transform playerTrans;
    private CapsuleCollider2D weaponCol;
    private BoxCollider2D monsterCol;

    [HideInInspector]
    public Transform monster;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = transform.root;
        weaponCol = GetComponent<CapsuleCollider2D>();
        monster = GameObject.FindGameObjectWithTag("Fox").transform;
        monsterCol = monster.GetComponent<BoxCollider2D>();
        stat = playerTrans.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag.CompareTo("Fox") == 0) &&
            (collision == monsterCol) &&
            (stat.curState == PlayerState.ATTACK))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Player Damage : " + stat.finalDamage);
                monster.SendMessage("ApplyDamage", stat.finalDamage);
            }
        }
    }
}
