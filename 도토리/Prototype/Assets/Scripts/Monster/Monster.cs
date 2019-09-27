using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float movePower;
    public float Hp;
    private float currentHp = 0;
    private int movementFlag = 0;
    private bool isChasing;
    public float nextTime = 0.0f;
    public float TimeLeft;

    public Transform firepoint;
    public GameObject FireBallPrefab;

    Animator animator;
    Vector3 movement;

    GameObject ChaseTarget;
    GameObject Player;

    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = Hp;
        
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        yield return new WaitForSeconds(3.0f);

        StartCoroutine("ChangeMovement");
    }

  


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            ChaseTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
            StartCoroutine("Shoot");
            isChasing = true;
            animator.SetBool("isMoving", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.position.x + 2.0f >= Player.transform.position.x || transform.position.x - 2.0f <= Player.transform.position.x)
            {
                isChasing = false;
                animator.SetBool("isMoving", false);
            }
            else
            {
                isChasing = true;
                animator.SetBool("isMoving", true);
            }
            //    isChasing = true;
            //animator.SetBool("isMoving", true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = false;
            StartCoroutine("ChangeMovement");
            StopCoroutine("Shoot");
        }

    }
    IEnumerator Shoot()
    {
        if (transform.position.x + 2.0f >= Player.transform.position.x || transform.position.x - 2.0f <= Player.transform.position.x)
        {
            Debug.Log("Flame");
            Instantiate(FireBallPrefab, firepoint.position, firepoint.rotation);
            yield return new WaitForSeconds(4.0f);
        }
        StartCoroutine("Shoot");
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Player.SendMessage("ApplyDamage", 100f);
    //        Debug.Log("Hit");
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        StartCoroutine("Shoot");
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")    
    //    {
    //        StopCoroutine("Shoot");
    //        StartCoroutine("ChangeMovement");
    //    }

    //}
    //// Update is called once per frame
    void FixedUpdate()
    {
        Move();
        //if (transform.position.x + 2.0f >= Player.transform.position.x || transform.position.x - 2.0f <= Player.transform.position.x)
        //{
        //    isChasing = false;
        //    animator.SetBool("isMoving", false);
        //}
        //else
        //{
        //    isChasing = true;
        //    animator.SetBool("isMoving", true);
        //}


    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isChasing)
        {
            Vector3 playerPos = ChaseTarget.transform.position;

            if (playerPos.x < transform.position.x)
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

    void ApplyDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }

    }
}
