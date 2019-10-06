using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public enum ElementalProperty
{
    None,
    Fire,
    Water,
    Grass
}


public class PlayerController : MonoBehaviour
{
    public PlayerStats stat;

    public int jumpCount = 0;

    public const float CONTROL_VALUE_OF_FALLSPEED = 2f;

    public bool isKeyInputting = false;
    public bool isGrounded;
    public bool isFalling;
    public int prevAttackDir = -1;
    public bool isAirColliderPassing;
    public bool isAirColliderPassingEnd;

    public int attackDir = 0;

    public float gravity;
    public float verticalVelocity;
    public float curAttackAnimSpeed;
    public const float maxAttackAnimTime = 0.5f;

    [HideInInspector]
    public Transform monster;

    public Vector3 mousePos;
    [HideInInspector]
    public Vector3 lastMoveDir = Vector3.zero;
    public Vector3 dashDir = Vector3.zero;
    private Vector2 moveDirection;
    public Rigidbody2D rigidbody2;
    public Animator anim;
    public CapsuleCollider2D playerCollider;
    public Transform spriteTrans;
    public Vector3 flipScale;

    public TilemapCollider2D stage1;

    public const float BOXCAST_DISTANCE = 0.35f;

    public ElementalProperty curWeaponProperty;

    public float curElementalDurantionTime;
    public float maxElementalDurantionTime;     //const

    public Dictionary<PlayerState, PlayerFSMController> states =
        new Dictionary<PlayerState, PlayerFSMController>();

    public int layerMask;

    private void Awake()
    {
        isAirColliderPassing = false;
        isAirColliderPassingEnd = false;

        playerCollider = GetComponent<CapsuleCollider2D>();
        isFalling = false;

        stat = GetComponent<PlayerStats>();

        verticalVelocity = 9.8f;

        rigidbody2 = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        spriteTrans = transform.GetChild(0);
        flipScale = spriteTrans.localScale;

        layerMask = 1 << 10 | 1 << 11;

        stage1 = GameObject.FindGameObjectWithTag("Stage").transform.GetChild(1).GetComponent<TilemapCollider2D>();
        monster = GameObject.FindGameObjectWithTag("Fox").transform;
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.WALK, GetComponent<PlayerWALK>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.ATTACK, GetComponent<PlayerATTACK>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.DEAD, GetComponent<PlayerDEAD>());
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(stat.startState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D)))
            SetState(PlayerState.WALK);

        if (!Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpCount == 0)
                    SetState(PlayerState.JUMP);
            }
        }

        if (curAttackAnimSpeed == 0)
        {
            if (Input.GetMouseButtonDown(0))
                SetState(PlayerState.ATTACK);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetState(PlayerState.DASH);

        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetState(PlayerState.DOWN);
            }
        }
            

        GroundCollisionCheck();
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
            {
                isKeyInputting = false;
            }
        }

        if (!isKeyInputting)
            SetState(PlayerState.IDLE);

        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        //sprite flip
        if (transform.position.x > mousePos.x)
        {
            spriteTrans.localScale = new Vector3(-flipScale.x, flipScale.y, flipScale.z);
        }
        else
        {
            spriteTrans.localScale = new Vector3(flipScale.x, flipScale.y, flipScale.z);
        }

        MakeEffects();
    }

    public void MakeEffects()
    {
        if (states[PlayerState.WALK].enabled && !states[PlayerState.JUMP].enabled)
        {
            EffectManager.Instance.SetEffect(transform.position.x, mousePos.x, (int)PlayerState.WALK - 1);
        }
        else if (states[PlayerState.ATTACK].enabled && curAttackAnimSpeed >= maxAttackAnimTime)     //anim end effect make
        {
            if (attackDir > -1)                                                                     //attack up
                EffectManager.Instance.SetEffect(transform.position.x, mousePos.x, (int)PlayerState.ATTACK - 1);
            else                                                                                    //attack down
                EffectManager.Instance.SetEffect(transform.position.x, mousePos.x, (int)PlayerState.ATTACK);
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

        stat.curState = newState;
        states[stat.curState].enabled = true;
        states[stat.curState].BeginState();

        if (states[PlayerState.JUMP].enabled)           //Jumping
            anim.SetInteger("curState", (int)PlayerState.JUMP);
        else
        {
            anim.SetInteger("curState", (int)stat.curState);

            if (states[PlayerState.ATTACK].enabled)
            {
                attackDir = prevAttackDir;

                if (attackDir < 1)                               //Prev Anim down attack
                {
                    Debug.Log("Up Attack!");
                    AttackDirCheck(1);
                }
                else                                            // Prev Anim up attack 
                {
                    Debug.Log("Down Attack!");
                    AttackDirCheck(-1);
                }

            }

        }

    }

    public void Gravity()
    {
        if (!isGrounded)
        {
            if (gravity >= -stat.fallSpeed / CONTROL_VALUE_OF_FALLSPEED)                //player FallSpeed control
                gravity -= stat.fallSpeed * Time.deltaTime;

            moveDirection = Vector2.zero;
            moveDirection.y = gravity * verticalVelocity;
            transform.Translate(moveDirection * Time.deltaTime);
        }
    }
    public void GroundCollisionCheck()
    {
        if (!states[PlayerState.JUMP].enabled)
        {
            if (stage1.enabled)
            {
                RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, -transform.up, BOXCAST_DISTANCE, layerMask);
                if (hit2D)
                {
                    isGrounded = true;
                }
                else
                    isGrounded = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit2D hit2D;
        //hit2D = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, -transform.up, BOXCAST_DISTANCE, layerMask);
        //if (hit2D)
        //{
        //    Gizmos.DrawRay(transform.position, -transform.up * hit2D.distance);
        //    Gizmos.DrawWireCube(transform.position + -transform.up * hit2D.distance, transform.lossyScale);
        //}
        //else
        //{
        //    Gizmos.DrawRay(transform.position, -transform.up * 1f);
        //}

        if (states[PlayerState.DOWN].enabled)
        {
            hit2D = Physics2D.Raycast(transform.position, -transform.up, 20f, layerMask);
            if (hit2D)
            {
                Gizmos.DrawRay(transform.position, -transform.up * 20f);
            }
        }

    }



    public void AttackDirCheck(int newDir)
    {
        if (newDir != 0)                               //attackDir Change
            attackDir = prevAttackDir = newDir;
        else                                          // attaked finish
            attackDir = newDir;

        if (states[PlayerState.ATTACK].enabled)
        {
            anim.SetInteger("attackDir", attackDir);
        }
    }

    void ApplyDamage(float damage)
    {
        stat.currentHP -= damage;

        if (stat.currentHP <= 0)
        {
            Debug.Log("PlayerDead");
        }
    }
}
