using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    IDLE = 0,
    DASH,
    ATTACK,
    JUMP,
    DEAD,
    DOWN
  
}

public class PlayerFSMManager : MonoBehaviour
{
    public PlayerState currState;
    public PlayerState startState;
    public CharacterController cc;
    public CharacterStat stat;
    public Animator anim;
    public SpriteRenderer mySpriteRenderer;

    public float MousePosX;
    public float verticalVelocity;

    public bool isFliped = false;

    public Vector2 moveDirection = Vector2.zero;
    public Vector3 lastMoveDir;

    Dictionary<PlayerState, PlayerFSMState> states = new Dictionary<PlayerState, PlayerFSMState>();
    // Start is called before the first frame update
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        stat = GetComponent<CharacterStat>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDir = Vector3.right;

        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.DEAD, GetComponent<PlayerDEAD>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.ATTACK, GetComponent<PlayerATTACK>());
    }
    private void Start()
    {
        SetState(startState);
    }
    public void SetState(PlayerState newState)
    {
        if (currState == PlayerState.DEAD)
        {
            return;
        }

        foreach (PlayerFSMState fsm in states.Values)
        {
            fsm.enabled = false;
        }

        currState = newState;
        states[currState].enabled = true;
        states[currState].BeginState();
        //anim.SetInteger("CurrentState", (int)currentState);
    }
    public void SetDead()
    {
        cc.enabled = false;
        SetState(PlayerState.DEAD);
    }


    // Update is called once per frame
    private void Update()
    {
        MousePosX = Input.mousePosition.x;
        Debug.Log(MousePosX);
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal") * stat.moveSpeed * Time.deltaTime, 0);

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Translate(new Vector2(moveDirection.x, 0));
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Translate(new Vector2(moveDirection.x, 0));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetState(PlayerState.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetState(PlayerState.JUMP);
        }

        Gravity();
    }

    public void Gravity()
    {
        if (cc.isGrounded)
        {
            verticalVelocity = -stat.gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.W))
            {
                verticalVelocity = stat.jumpForce;
                SetState(PlayerState.JUMP);
            }
        }
        else
        {
            verticalVelocity -= stat.gravity * Time.deltaTime;
        }
        moveDirection = Vector2.zero;
        moveDirection.y = verticalVelocity;
        cc.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.gameObject.tag == "Ground") && (verticalVelocity <= -stat.gravity))
        {
            SetState(PlayerState.IDLE);
        }
    }
}
