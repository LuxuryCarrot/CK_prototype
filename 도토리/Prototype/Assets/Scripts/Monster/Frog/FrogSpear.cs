using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpear : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("SpearAttack");
        Player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DestroySpear", 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SendMessage("ApplyDamage", 0.5f);
        }
    }

    void DestroySpear()
    {
        Destroy(gameObject);
        //Debug.Log("SpearDestroied");
    }
}
