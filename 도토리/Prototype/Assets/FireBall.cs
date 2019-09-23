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
 
    // Start is called before the first frame update
    void Start()
    {
        Fox = GameObject.FindGameObjectWithTag("Fox");
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Ps = GetComponentInChildren<ParticleSystem>();
        if (transform.position.x < Fox.transform.position.x)
        {
            rb.velocity = transform.right * speed;
        }
        if (transform.position.x > Fox.transform.position.x)
        {
            Ps.transform.localScale *= new Vector2(-1, 1);
            rb.velocity = transform.right * -1 * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Player.SendMessage("ApplyDamage", 1.0f);
        }
    }
    private void Update()
    {
        if (transform.position.x > 20.0f || transform.position.x < -20.0f)
    	{
            Destroy(gameObject);
	    }
        
    }


}
