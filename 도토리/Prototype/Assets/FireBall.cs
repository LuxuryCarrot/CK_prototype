using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    GameObject Player;
    GameObject Fox;
    ParticleSystem Ps;

    CapsuleCollider2D m_CapsuleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("fireball");
        Fox = GameObject.FindGameObjectWithTag("Fox");
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Ps = GetComponentInChildren<ParticleSystem>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();

        // shooting fireball code 
        //if (transform.position.x > Fox.transform.position.x)
        //{
        //    rb.velocity = transform.right * speed;
        //}
        //else
        //{
        //    Ps.transform.localScale *= new Vector2(-1, 1);
        //    rb.velocity = transform.right * -1 *speed;
        //}

        Invoke("FireBallColliderDestroy", 1.0f);
        Invoke("FireBallDestroy", 2.0f);

        //transform.position = new Vector2(Fox.transform.position.x+2, Fox.transform.position.y + 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_CapsuleCollider2D.enabled = false;
            StartCoroutine("Flame");
        }
        else
        {
            StopCoroutine("Flame");
        }
    }
    IEnumerator Flame()
    {
        Player.SendMessage("ApplyDamage", 1.0f);
        Debug.Log("fireball hit");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Flame");
    }
    void FireballColliderDestroy()
    {
        m_CapsuleCollider2D.enabled = false;
    }
    void FireBallDestroy()
    {
        Destroy(gameObject);
    }
}
