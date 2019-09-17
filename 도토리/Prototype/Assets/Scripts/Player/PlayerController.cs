﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE=0,
    WALK,
    JUMP,
    DOWN,
    DASH,
    ATTACK
}


public class PlayerController : MonoBehaviour
{
    private bool isKeyInput = false;
    private float fallSpeed;
    public float verticalVelocity;
    private float gravity;
    public float walkSpeed;
    public float dashSpeed;
    public float jumpForce;

    [HideInInspector]
    public Vector3 mousePos;
    [HideInInspector]
    public Vector3 lastMoveDir = Vector3.zero;
    [HideInInspector]
    public Vector3 dashDir = Vector3.zero;
    [SerializeField]
    private Vector2 moveDirection;
    public CharacterController cc;
    public Animator anim;
    public SpriteRenderer sprite;

    public PlayerState startState;
    public PlayerState curState;

    public Dictionary<PlayerState, PlayerFSMController> states =
        new Dictionary<PlayerState, PlayerFSMController>();


    private void Awake()
    {
        jumpForce =1.5f;
        walkSpeed = 3f;
        dashSpeed = 3f;
        fallSpeed = 4f;
        verticalVelocity = 9.8f;
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.WALK, GetComponent<PlayerWALK>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.ATTACK, GetComponent<PlayerATTACK>());
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(fallSpeed);
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


        for(int i=1; i<states.Count;)
        {
            if (states[(PlayerState)i].enabled == false)
            {
                i++;
            }
            else
                break;

            if (i == states.Count)
                isKeyInput = false;
        }

        if (!isKeyInput)
            SetState(PlayerState.IDLE);

        Gravity();

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            isKeyInput = true;
            states[PlayerState.IDLE].enabled = false;
        }

        if (!isKeyInput)
        {
            foreach (PlayerFSMController fsm in states.Values)
            {
                fsm.enabled = false;
            }
        }

        curState = newState;
        states[curState].enabled = true;

        if (states[PlayerState.JUMP].enabled)
            anim.SetInteger("curState", (int)PlayerState.JUMP);
        else
            anim.SetInteger("curState", (int)curState);
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
}
