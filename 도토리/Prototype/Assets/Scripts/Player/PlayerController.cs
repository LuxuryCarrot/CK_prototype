using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE = 0,
    WALK,
    JUMP,
    DOWN,
    DASH,
    ATTACK,
    DEAD
}

public enum ElementalProperty
{
    None,
    Fire,
    Water,
    Grass
}


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool isKeyInputting = false;

    public bool isUpAttacked = true;

    public float attackDir = 0;

    public float walkSpeed;

    private float gravity;
    public float verticalVelocity;
    private float fallSpeed;
    public float jumpForce;

    public float dashSpeed;
    public float dashForce;


    [HideInInspector]
    public Transform monster;

    [SerializeField]
    public Vector3 mousePos;
    [HideInInspector]
    public Vector3 lastMoveDir = Vector3.zero;
    public Vector3 dashDir = Vector3.zero;
    private Vector2 moveDirection;
    public CharacterController cc;
    public Animator anim;
    public SpriteRenderer sprite;

    public Quaternion startAngle;

    public GameObject weapon;

    public PlayerState startState;
    public PlayerState curState;

    public ElementalProperty curWeaponProperty;

    public float hp;
    public float currentHP;
    public float weaponDamage;
    public float curElementalDurantionTime;
    public float maxElementalDurantionTime;     //const

    public Dictionary<PlayerState, PlayerFSMController> states =
        new Dictionary<PlayerState, PlayerFSMController>();


    private void Awake()
    {
        jumpForce = 1.5f;
        walkSpeed = 3f;
        dashSpeed = 6f;
        dashForce = 1.2f;
        fallSpeed = 4f;
        verticalVelocity = 9.8f;
        weaponDamage = 10f;
        hp = 100;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        monster = GameObject.FindGameObjectWithTag("Fox").transform;
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.WALK, GetComponent<PlayerWALK>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.ATTACK, GetComponent<PlayerATTACK>());
        states.Add(PlayerState.DEAD, GetComponent<PlayerDEAD>());
    }

    // Start is called before the first frame update
    void Start()
    {
        startAngle = weapon.transform.rotation;
        currentHP = hp;
        SetState(startState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D)))
            SetState(PlayerState.WALK);
        if (Input.GetKey(KeyCode.S))
            SetState(PlayerState.DOWN);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetState(PlayerState.DASH);
        if (Input.GetMouseButtonDown(0))
            SetState(PlayerState.ATTACK);

        Gravity();

        for (int i = 1; i < states.Count;)
        {
            if (states[(PlayerState)i].enabled == false)
            {
                i++;
            }
            else
                break;

            if (i == states.Count)
                isKeyInputting = false;
        }

        if (!isKeyInputting)
            SetState(PlayerState.IDLE);

        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        //sprite flip
        if (transform.position.x > mousePos.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }


    public void SetState(PlayerState newState)
    {
        if (newState != PlayerState.IDLE)
        {
            isKeyInputting = true;
            states[PlayerState.IDLE].enabled = false;
        }

        if (!isKeyInputting)
        {
            foreach (PlayerFSMController fsm in states.Values)
            {
                fsm.enabled = false;
            }
        }

        curState = newState;
        states[curState].enabled = true;
        states[curState].BeginState();

        if (states[PlayerState.JUMP].enabled)           //Jumping
            anim.SetInteger("curState", (int)PlayerState.JUMP);
        else
            anim.SetInteger("curState", (int)curState);

        if (states[PlayerState.ATTACK].enabled)
            anim.SetFloat("attackDir", attackDir);
    }

    public void Gravity()
    {
        if (cc.isGrounded)
        {
            gravity = -fallSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.W))
            {
                gravity = jumpForce;
                SetState(PlayerState.JUMP);
            }
        }
        else
        {
            gravity -= fallSpeed * Time.deltaTime;
        }
        moveDirection = Vector2.zero;
        moveDirection.y = gravity * verticalVelocity;
        cc.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.gameObject.tag == "Ground") &&
            (gravity <= fallSpeed))
        {
            states[PlayerState.JUMP].enabled = false;
        }
    }

    public void AttackDirCheck(bool newDir)
    {
        attackDir = newDir ? 1 : 0;
    }

    void ApplyDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {

        }
    }
}
