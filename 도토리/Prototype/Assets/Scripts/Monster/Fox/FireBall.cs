﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    GameObject Player;
    ParticleSystem Ps;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("fireball");
        Player = GameObject.FindGameObjectWithTag("Player");
        Ps = GetComponentInChildren<ParticleSystem>();
   
        if (transform.position.x < Player.transform.position.x)
        {
            Ps.transform.localScale *= new Vector2(-1, 1);
        }
        
        
        //Invoke("FireBallColliderDestroy", 1.0f);
        Invoke("FireBallDestroy", 2.0f);

        //transform.position = new Vector2(Fox.transform.position.x+2, Fox.transform.position.y + 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("Flame");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("Flame");
        }
    }
    IEnumerator Flame()
    {
        Player.SendMessage("ApplyDamage", 1.0f);
        Debug.Log("Flame damage 1.0f");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Flame");
    }
    //void FireballColliderDestroy()
    //{
    //    m_CapsuleCollider2D.enabled = false;
    //}
    void FireBallDestroy()
    {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
}
