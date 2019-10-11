﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public PlayerStats stat;

    public int jumpCount = 0;

    public const float CONTROL_VALUE_OF_FALLSPEED = 2f;

    public bool gracePeriodEnable = false;

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
    public const float MAX_ATTACK_ANIM_TIME = 0.5f;      // + effect time
    public float curShotAnimSpeed;
    public const float MAX_SHOT_ANIM_TIME = 1f;

    public float boxScale = 100;

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
    public Material whiteMat;
    public Material originalMat;


    public TilemapCollider2D tileMaplCollider;

    public const float BOXCAST_DISTANCE = 0.5f;


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
        boxScale = 500f;
        rigidbody2 = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        spriteTrans = transform.GetChild(0);
        flipScale = spriteTrans.localScale;

        layerMask = 1 << 10 | 1 << 11;

        tileMaplCollider = GameObject.FindGameObjectWithTag("Stage").transform.GetChild(1).GetComponent<TilemapCollider2D>();
        //monster = GameObject.FindGameObjectWithTag("Monster").transform;
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.WALK, GetComponent<PlayerWALK>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.ATTACK, GetComponent<PlayerATTACK>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.DEAD, GetComponent<PlayerDEAD>());
        states.Add(PlayerState.SHOT, GetComponent<PlayerSHOT>());
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(stat.startState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPlayerDead)
        {
            CommandCheck();

            KeyInputtingCheck();

            SetMousePosition();

            SpriteFlipCheck();

            MakeEffects();
        }
        else
        {
            if (stat.curState != PlayerState.DEAD)                              //한번만 실행
                SetState(PlayerState.DEAD);
        }
        GroundCollisionCheck();

        Gravity();
    }

    public void CommandCheck()
    {

        if (curAttackAnimSpeed == 0 && !EffectManager.Instance.isAttackEffectPlaying)      //공격의 애니메이션,이펙트의 재생이 종료되면 제어가능
        {
            if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D)))       //걷기
                SetState(PlayerState.WALK);

            if (!Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.Space))                        //점프    
                {
                    if (jumpCount == 0)
                        SetState(PlayerState.JUMP);
                }
            }

            if (!gracePeriodEnable)                                            //피격중에는 공격 불가
            {
                if (Input.GetMouseButtonDown(0))                                //공격
                    SetState(PlayerState.ATTACK);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))                        //대시
                SetState(PlayerState.DASH);

            if (Input.GetKey(KeyCode.S))                                    //하강점프          
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetState(PlayerState.DOWN);
                }
            }
        }

    }

    public void KeyInputtingCheck()
    {
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
        {
            if (curShotAnimSpeed == 0)             //피격중에는 기다림
                SetState(PlayerState.IDLE);
        }
    }

    public void SetMousePosition()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
    }

    public void SpriteFlipCheck()
    {
        //sprite flip
        if (transform.position.x > mousePos.x)
        {
            spriteTrans.localScale = new Vector3(-flipScale.x, flipScale.y, flipScale.z);
        }
        else
        {
            spriteTrans.localScale = new Vector3(flipScale.x, flipScale.y, flipScale.z);
        }
    }

    public void MakeEffects()
    {
        if (states[PlayerState.WALK].enabled && !states[PlayerState.JUMP].enabled)
        {
            EffectManager.Instance.SetStateEffect(transform.position.x, mousePos.x, (int)PlayerState.WALK - 1);
        }
        else if (states[PlayerState.ATTACK].enabled && curAttackAnimSpeed >= MAX_ATTACK_ANIM_TIME)     //anim end effect make
        {
            if (attackDir > -1)                                                                     //attack up
                EffectManager.Instance.SetStateEffect(transform.position.x, mousePos.x, (int)PlayerState.ATTACK - 1);
            else                                                                                    //attack down
                EffectManager.Instance.SetStateEffect(transform.position.x, mousePos.x, (int)PlayerState.ATTACK);
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
        SetAnimation();                                     //애니메이션 설정
    }

    public void SetAnimation()
    {
        if (!isGrounded)                                            //공중에 떠있을때는 점프 애니메이션 실행
        {
            anim.SetInteger("curState", (int)PlayerState.JUMP);
        }
        else
        {
            anim.SetInteger("curState", (int)stat.curState);

            if (states[PlayerState.ATTACK].enabled)
            {
                attackDir = prevAttackDir;

                if (attackDir < 1)                               //Prev Anim down attack
                {
                    AttackDirCheck(1);
                }
                else                                            // Prev Anim up attack 
                {
                    AttackDirCheck(-1);
                }
            }
        }
    }

    public void Gravity()
    {
        if (!states[PlayerState.DASH].enabled)
        {
            if (!isGrounded)
            {
                if (gravity >= -stat.fallSpeed / CONTROL_VALUE_OF_FALLSPEED)                //player FallSpeed control
                    gravity -= stat.fallSpeed * Time.deltaTime;

                moveDirection = Vector2.zero;
                moveDirection.y = gravity * verticalVelocity;
                transform.Translate(moveDirection * Time.deltaTime);
            }
            else
                gravity = 0;
        }
    }


    public void GroundCollisionCheck()
    {
        if (!states[PlayerState.JUMP].enabled && !states[PlayerState.DOWN].enabled && !states[PlayerState.DASH].enabled)
        {
            if (tileMaplCollider.enabled)
            {
                RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, new Vector2(0.4f, transform.lossyScale.y / 1000), 0, -transform.up, BOXCAST_DISTANCE, layerMask);

                if (hit2D)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
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
        if (!GameManager.Instance.isPlayerDead)
        {
            if (!gracePeriodEnable)
            {
                stat.currentHP -= damage;
                EffectManager.Instance.SetPlayerShotEffect();
                SetState(PlayerState.SHOT);
                StartCoroutine("GracePeriod");
                if (stat.currentHP <= 0)
                {
                    Debug.Log("PlayerDead");
                    GameManager.Instance.isPlayerDead = true;
                }
            }
        }
    }

    IEnumerator GracePeriod()
    {
        gracePeriodEnable = true;

        int countTime = 0; //깜빡이는 횟수
        while (countTime < 5)
        {
            if (countTime % 2 == 0)
            {
                transform.GetComponentInChildren<SpriteRenderer>().material = originalMat;
                transform.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
            }
            else
            {
                transform.GetComponentInChildren<SpriteRenderer>().material = whiteMat;
                transform.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
            }
            yield return new WaitForSeconds(0.1f);
            countTime++;
        }

        transform.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        gracePeriodEnable = false;
    }
}
