using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster_Frog : Monster
{
    public float movePower;
    public float Hp;

    private float currentHp = 0;

    public int movementFlag = 0;
    public int FrogShieldCount;

    private int RandNum;
    private int AnimState;

    public bool AttackRange = true;
    public bool isDead;

    private bool isChasing;

    public Transform firepoint;

    public GameObject SpearPrefab;
    public GameObject CoreItemPrefab;

    private GameObject FrogShield;
    private GameObject FrogStartSprite;

    Animator animator;

    Vector3 movement;

    GameObject ChaseTarget;
    GameObject Player;

    CharacterController cc;


    // Start is called before the first frame update
    void Awake()
    {
        animalname = AnimalName.FROG;

        FrogStartSprite = transform.GetChild(2).gameObject;
        Destroy(FrogStartSprite);

        isDead = false;

        currentHp = Hp;
        AnimState = 0;

        cc = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player");

        FrogShield = transform.GetChild(0).gameObject;

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

        if (movementFlag == 0)
        {
            AnimState = 0;
        }

        if (FrogShieldCount == 0)
        {
            Debug.Log("Shield Destroyed");
            Destroy(FrogShield);
        }

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

    private void CoreItemDrop()
    {
        RandNum = Random.Range(0, 9);
        if (RandNum == 0 || RandNum == 1 || RandNum == 2)
        {
            Instantiate(CoreItemPrefab, transform.position, transform.rotation);
        }
    }

    IEnumerator FirstAttack()
    {
        Debug.Log("FirstAttackStart");
        AnimState = 2;
        yield return new WaitForSeconds(0.25f);
        AnimState = 0;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine("Attack");
        StopCoroutine("FirstAttack");
    }

    IEnumerator Attack()
    {
        AnimState = 2;
        Instantiate(SpearPrefab, firepoint.position, firepoint.rotation);
        yield return new WaitForSeconds(1.0f);
        AnimState = 0;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine("Attack");
    }

    IEnumerator DestroyMonster()
    {
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
        if (FrogShieldCount > 0)
        {
            Debug.Log("Shield count -1");
            Debug.Log("Shield Count =" + FrogShieldCount);
            FrogShieldCount -= 1;
        }
        else if (FrogShieldCount >= 0)
        {
            AnimState = 3;
            Debug.Log("Damage =" + damage);
            currentHp -= damage;
            Debug.Log("Left Frog HP = " + currentHp);
        }

        if (currentHp <= 0)
        {
            isDead = true;
        }
    }
}
