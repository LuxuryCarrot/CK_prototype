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
        transform.position = new Vector2(Fox.transform.position.x+2, Fox.transform.position.y + 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Player.SendMessage("ApplyDamage", 1.0f);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, 2 * Time.deltaTime);

        if (transform.position.x > 20.0f || transform.position.x < -20.0f)
    	{
            Destroy(gameObject);
	    }
        
        
    }


}
