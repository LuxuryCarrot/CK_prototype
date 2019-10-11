using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Raccoon : MonoBehaviour
{
    public float movePower;
    public float Hp;

    public int movementFlag = 0;
    public bool AttackRange = true;

    private bool isDead;
    private bool isChasing;
    public float currentHp = 0;

    public Transform SwordPoint;

    private GameObject FrogStartSprite;

    Animator animator;
    Vector3 movement;

    public GameObject SwordPrefab;
    GameObject ChaseTarget;
    GameObject Player;

    CharacterController cc;
    // Start is called before the first frame update
    void Awake()
    {
        FrogStartSprite = transform.GetChild(0).gameObject;
        Destroy(FrogStartSprite);
        isDead = false;

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
            animator.SetInteger("moveMentFlag", 0);
        }
        else
        {
            animator.SetInteger("moveMentFlag", 1);
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
            isChasing = true;
            StopCoroutine("Attack");
            animator.SetInteger("moveMentFlag", 1);
            AttackRange = true;
        }
        if ((transform.position.x + 2.0f >= Player.transform.position.x && transform.position.x - 2.0f <= Player.transform.position.x)
            && (transform.position.y + 2.0f >= Player.transform.position.y && transform.position.y - 1.0f <= Player.transform.position.y))
        {
            if (AttackRange == true)
            {
                StartCoroutine("FirstAttack");
                AttackRange = false;
            }
        }
        else
        {
            animator.SetInteger("moveMentFlag", 1);
            Move();
        }

        if (isDead == true)
        {
            StartCoroutine("DestroyMonster");
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
        }

        if (other.gameObject.tag == "GodHand")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator FirstAttack()
    {
        Debug.Log("FirstAttackStart");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Attack");
        StopCoroutine("FirstAttack");
    }

    IEnumerator Attack()
    {
        animator.SetInteger("moveMentFlag", 2);
        Instantiate(SwordPrefab, SwordPoint.position, SwordPoint.rotation);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Attack");
    }
    IEnumerator DestroyMonster()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SendMessage("ApplyDamage", 0.5f);
            Debug.Log("몸통박치기 데미지");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = false;
            StartCoroutine("ChangeMovement");
            StopCoroutine("Attack");
        }

    }

    void ApplyDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            animator.SetInteger("moveMentFlag", 4);
            isDead = true;
        }
    }
}
