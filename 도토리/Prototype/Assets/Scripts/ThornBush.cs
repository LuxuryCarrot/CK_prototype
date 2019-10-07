using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBush : MonoBehaviour
{
    GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit");
            Player.SendMessage("ApplyDamage", 1.0f);
        }
    }
}
