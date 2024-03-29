﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Monster_Fox : Monster
{
    public float movePower;
    public float Hp;

    private float currentHp = 0;

    public int movementFlag = 0;

    private int RandNum;
    private int AnimState;

    public bool isDead;
    public bool AttackRange;
    public bool isTutorial;

    private bool isChasing;

    public Image HpBar;

    public Transform firepoint;

    public GameObject FireBallPrefab;
    public GameObject CoreItemPrefab;

    private GameObject FoxStartSprite;
    private GameObject ChaseTarget;
    private GameObject Player;

    Animator animator;

    Vector3 movement;

    CircleCollider2D CircleCollider2;

    BoxCollider2D boxCollider2D;

    CharacterController cc;
    // Start is called before the first frame update
    void Awake()
    {
        animalname = AnimalName.FOX;

        FoxStartSprite = transform.GetChild(1).gameObject;
        Destroy(FoxStartSprite);

        AnimState = 0;
        currentHp = Hp;

        AttackRange = true;
        isDead = false;

        boxCollider2D = GetComponent<BoxCollider2D>();

        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player");

        CircleCollider2 = GetComponent<CircleCollider2D>();

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
        {
            AnimState = 0;
        }
        else
        {
            AnimState = 1;
        }
        yield return new WaitForSeconds(3.0f);

        StartCoroutine("ChangeMovement");
    }


    void FixedUpdate()
    {
        animator.SetInteger("CurrentState", AnimState);

        if ((transform.position.x + 5.0f >= Player.transform.position.x && transform.position.x - 5.0f <= Player.transform.position.x)
            && (transform.position.x + 2.0f <= Player.transform.position.x || transform.position.x - 2.0f >= Player.transform.position.x)
            && (transform.position.y + 2.0f >= Player.transform.position.y && transform.position.y - 1.0f <= Player.transform.position.y))
        {
            isChasing = true;
            StopCoroutine("Shoot");
            AnimState = 1;
            AttackRange = true;
        }
        if ((transform.position.x + 2.0f >= Player.transform.position.x && transform.position.x - 2.0f <= Player.transform.position.x) 
            && (transform.position.y + 2.0f >= Player.transform.position.y && transform.position.y - 1.0f <= Player.transform.position.y))
        {
            if (AttackRange == true)
            {
                StartCoroutine("Shoot");
                AttackRange = false;
            }
        }
        else
        {
             Move(); 
        }

        if (isDead)
        {
            StartCoroutine("DestroyMonster");
            StopCoroutine("ChangeMovement");
            isChasing = false;
            movementFlag = 0;
        }

    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isChasing)
        {
            Vector3 playerPos = Player.transform.position;

            if (playerPos.x <= transform.position.x)
            {

                dist = "Left";
                
            }
            else if (playerPos.x > transform.position.x)
            {

                dist = "Right";
            }
        }
        else
        {
            if (movementFlag == 1)
            {
                dist = "Left";
               
            }
            else if (movementFlag == 2)
            {
                dist = "Right";
              
            }
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ChaseTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
            //isChasing = true;
            //animator.SetBool("isMoving", true);
            CircleCollider2.enabled = false;
        }

        if (other.gameObject.tag == "GodHand")
        {
            isDead = true;
        }
    }

    IEnumerator Shoot()
    {
        Debug.Log("Flame");
        AnimState = 2;
        Instantiate(FireBallPrefab, firepoint.position, firepoint.rotation);
        yield return new WaitForSeconds(1.0f);
        AnimState = 0;
        yield return new WaitForSeconds(3.0f);

        StartCoroutine("Shoot");
    }

    IEnumerator DestroyMonster()
    {
        boxCollider2D.enabled = false;
        AnimState = 4;
        yield return new WaitForSeconds(1.3f);
        CoreItemDrop();
        Destroy(gameObject);
        StopCoroutine("DestroyMonster");
    }

    
    private void CoreItemDrop()
    {
        RandNum = Random.Range(0, 9);
        if(isTutorial)
        {
            Debug.Log("isTutorial Stage");
            Instantiate(CoreItemPrefab, transform.position, transform.rotation);
        }
        else if (!isTutorial)
        {
            Debug.Log("RandNum = " + RandNum);
            if (RandNum == 0 || RandNum == 1 || RandNum == 2)
            {
                Instantiate(CoreItemPrefab, transform.position, transform.rotation);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SendMessage("ApplyBodyDamage", 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = false;
            StartCoroutine("ChangeMovement");
            StopCoroutine("Shoot");
            CircleCollider2.enabled = true;
        }

    }
    void ApplyDamage(float damage)
    {
        AnimState = 3;

        currentHp -= damage;

        HpBar.fillAmount -= damage / Hp;

        if (currentHp <= 0)
        {
            isDead = true;
        }
    }
}
