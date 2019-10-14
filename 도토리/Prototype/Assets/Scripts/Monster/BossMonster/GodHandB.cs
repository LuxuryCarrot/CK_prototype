using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandB : MonoBehaviour
{
    private GameObject Player;


    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DestroyArmB", 0.5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PlayerAttacked");
            Player.SendMessage("ApplyDamage", 1.0f);
        }
    }
    void DestroyArmB()
    {
        Destroy(gameObject);
    }
}
