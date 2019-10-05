using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Fox : MonoBehaviour
{
    public float movePower;
    public float Hp;
    private float currentHp = 0;

    public int movementFlag = 0;
    private bool isChasing;
    public bool AttackRange = true;

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


    void FixedUpdate()
    {
        if ((transform.position.x + 5.0f >= Player.transform.position.x && transform.position.x - 5.0f <= Player.transform.position.x)
            && (transform.position.x + 2.0f <= Player.transform.position.x || transform.position.x - 2.0f >= Player.transform.position.x)
            && (transform.position.y + 2.0f >= Player.transform.position.y && transform.position.y - 1.0f <= Player.transform.position.y))
        {
            Debug.Log("Check");
            isChasing = true;
            StopCoroutine("Shoot");
            animator.SetBool("isMoving", true);
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
        }
    }

    IEnumerator Shoot()
    {
        Debug.Log("Flame");
        Instantiate(FireBallPrefab, firepoint.position, firepoint.rotation);
        yield return new WaitForSeconds(4.0f);
        StartCoroutine("Shoot");
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
    void ApplyDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
