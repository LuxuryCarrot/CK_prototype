using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Monster_Raccoon : Monster
{
    public float movePower;
    public float Hp;
    public float currentHp = 0;

    public int movementFlag = 0;

    private int RandNum;
    private int AnimState;

    public bool AttackRange = true;
    public bool isDead;

    private bool isChasing;

    public Image HpBar;

    public Transform SwordPoint;

    public GameObject CoreItemPrefab;
    public GameObject SwordPrefab;

    private GameObject FrogStartSprite;

    Animator animator;
    Vector3 movement;

    GameObject ChaseTarget;
    GameObject Player;

    BoxCollider2D boxCollider2D;

    CharacterController cc;
    // Start is called before the first frame update
    void Awake()
    {
        animalname = AnimalName.RACCOON;

        FrogStartSprite = transform.GetChild(0).gameObject;

        Destroy(FrogStartSprite);

        isDead = false;

        currentHp = Hp;
        AnimState = 0;

        boxCollider2D = GetComponent<BoxCollider2D>();

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
        animator.SetInteger("moveMentFlag", AnimState);

        if ((transform.position.x + 5.0f >= Player.transform.position.x && transform.position.x - 5.0f <= Player.transform.position.x)
            && (transform.position.x + 2.0f <= Player.transform.position.x || transform.position.x - 2.0f >= Player.transform.position.x)
            && (transform.position.y + 2.0f >= Player.transform.position.y && transform.position.y - 1.0f <= Player.transform.position.y))
        {
            isChasing = true;
            StopCoroutine("Attack");
            AnimState = 1;
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
            AnimState = 1;
            Move();
        }

        if (isDead == true)
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

    private void CoreItemDrop()
    {
        RandNum = Random.Range(0, 9);
        if (RandNum == 0 || RandNum == 1 || RandNum == 2)
        {
            Instantiate(CoreItemPrefab, transform.position, transform.rotation);
        }
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
            isDead = true;
        }
    }
    IEnumerator FirstAttack()
    {
        AnimState = 2;
        Debug.Log("FirstAttackStart");
        yield return new WaitForSeconds(0.25f);
        AnimState = 0;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine("Attack");
        StopCoroutine("FirstAttack");
    }

    IEnumerator Attack()
    {
        AnimState = 2;
        Instantiate(SwordPrefab, SwordPoint.position, SwordPoint.rotation);
        yield return new WaitForSeconds(0.75f);
        AnimState = 0;
        yield return new WaitForSeconds(0.75f);
        StartCoroutine("Attack");
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SendMessage("ApplyBodyDamage", 0.5f);
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
        AnimState = 3;

        currentHp -= damage;

        HpBar.fillAmount -= damage / Hp;

        if (currentHp <= 0)
        {
            isDead = true;
        }
    }
}
