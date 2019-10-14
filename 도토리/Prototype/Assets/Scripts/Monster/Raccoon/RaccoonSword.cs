using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonSword : MonoBehaviour
{
    GameObject Player;
    ParticleSystem Ps;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("SwordAttack");
        Ps = GetComponentInChildren<ParticleSystem>();

        Player = GameObject.FindGameObjectWithTag("Player");

        if (transform.position.x < Player.transform.position.x)
        {
            Ps.transform.localScale *= new Vector2(-1, 1);
        }

        Invoke("DestroySpear", 0.5f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SendMessage("ApplyDamage", 1.0f);
        }
    }

    void DestroySpear()
    {
        Destroy(gameObject);
        //Debug.Log("SwordDestroied");
    }
}
