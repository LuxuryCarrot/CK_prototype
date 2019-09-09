using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    IDLE = 0,
    Walk,
    DASH,
    ATTACK,
    JUMP,
    DEAD
  
}

public class PlayerFSMManager : MonoBehaviour
{
    public PlayerState currState;
    public PlayerState startState;
    public CharacterController cc;
    public CharacterStat stat;
    public Animator anim;
    public SpriteRenderer mySpriteRenderer;

    public bool isMoving = false;
    public bool isFliped = false;

    public Vector2 moveDirection = Vector2.zero;

    Dictionary<PlayerState, PlayerFSMState> states = new Dictionary<PlayerState, PlayerFSMState>();
    // Start is called before the first frame update
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        stat = GetComponent<CharacterStat>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.DEAD, GetComponent<PlayerDEAD>());
        states.Add(PlayerState.Walk, GetComponent<PlayerWALK>());
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
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal") * stat.moveSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isMoving = true;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;
        }
        if (isMoving == true)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                mySpriteRenderer.flipX = true;
                transform.Translate(new Vector2(moveDirection.x, 0));
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                mySpriteRenderer.flipX = false;
                transform.Translate(new Vector2(moveDirection.x, 0));
            }
            SetState(PlayerState.Walk);
            Debug.Log("Moving");
        }
        
    }
}
