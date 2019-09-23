using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float movePower;
    public float Hp;
    public float fireballspd;
    private float currentHp = 0;
    

    private int movementFlag = 0;
    private bool isChasing;
    GameObject FireBall;
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

        FireBall = GameObject.FindGameObjectWithTag("FireBall");
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

    void ShootFireBall()
    {
        FireBall.transform.position = new Vector2(transform.position.x, transform.position.y);
        FireBall.transform.Translate(new Vector2(fireballspd * Time.deltaTime, 0));
        Debug.Log("파이어볼");
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ChaseTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
            Debug.Log(fireballspd);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = true;
            animator.SetBool("isMoving", true);
            if ()
            {

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")    
        {
            isChasing = false;
            StartCoroutine("ChangeMovement");
            StopCoroutine("ShootFireBall");

        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
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
            fireballspd *= 1;
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
            fireballspd *= -1;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void ApplyDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {

        }

    }
}
