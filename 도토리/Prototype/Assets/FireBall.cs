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

    CircleCollider2D m_CircleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("fireball");
        Fox = GameObject.FindGameObjectWithTag("Fox");
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Ps = GetComponentInChildren<ParticleSystem>();
        m_CircleCollider2D = GetComponent<CircleCollider2D>();

        if (transform.position.x > Fox.transform.position.x)
        {
            rb.velocity = transform.right * speed;
        }
        else
        {
            Ps.transform.localScale *= new Vector2(-1, 1);
            rb.velocity = transform.right * -1 *speed;
        }
        Invoke("FireBallDestroy", 2.0f);

        //transform.position = new Vector2(Fox.transform.position.x+2, Fox.transform.position.y + 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_CircleCollider2D.enabled = false;
            Debug.Log("fireball hit");
            Player.SendMessage("ApplyDamage", 1.0f);
        }
    }
    private void Update()
    {

        if (transform.position.x > Fox.transform.position.x + 1.5f || transform.position.x < Fox.transform.position.x - 1.5f)
    	{
            m_CircleCollider2D.enabled = false;
        }
        
        
    }

    void FireBallDestroy()
    {
        Destroy(gameObject);
    }
}
