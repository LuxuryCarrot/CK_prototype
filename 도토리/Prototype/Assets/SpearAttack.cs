using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    GameObject Player;
    ParticleSystem Ps;
    CapsuleCollider2D m_capsuleCollider2D;
    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        //Ps = GetComponentInChildren<ParticleSystem>();
        m_capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        if (transform.position.x < Player.transform.position.x)
        {
            //Ps.transform.localScale *= new Vector2(-1, 1);
        }
        Invoke("SpearDestroy", 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
    IEnumerator Damage()
    {
        Player.SendMessage("ApplyDamage", 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    void SpearDestroy()
    {
        StopCoroutine("Damage");
        Destroy(gameObject);
    }
}
