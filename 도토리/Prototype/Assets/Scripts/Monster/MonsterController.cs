using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    IDLE = 0,
    WALK,
    ATTACK,
    DEAD,
    CHASE
}
public class MonsterController : MonoBehaviour
{
    private bool isKeyInput = false;
    private float walkSpeed = 0;

    public Animator anim;
    public SpriteRenderer sprite;

    public MonsterState startState;
    public MonsterState curState;

    public Dictionary<MonsterState, MonsterFSMController> states = 
        new Dictionary<MonsterState, MonsterFSMController>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        states.Add(MonsterState.IDLE, GetComponent<MonsterIDLE>());
        states.Add(MonsterState.WALK, GetComponent<MonsterWALK>());
        states.Add(MonsterState.ATTACK, GetComponent<MonsterATTACK>());
        states.Add(MonsterState.DEAD, GetComponent<MonsterDEAD>());
        states.Add(MonsterState.CHASE, GetComponent<MonsterCHASE>());
    }
    // Start is called before the first frame update
    void Start()
    {
        SetState(startState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState(MonsterState newState)
    {
        if (newState != MonsterState.IDLE)
        {
            isKeyInput = true;
            states[MonsterState.IDLE].enabled = false;
        }

        if (!isKeyInput)
        {
            foreach (MonsterFSMController fsm in states.Values)
            {
                fsm.enabled = false;
            }
        }

        curState = newState;
        states[curState].enabled = true;
        states[curState].BeginState();
        if (states[MonsterState.WALK].enabled)
        {
            anim.SetInteger("curState", (int)MonsterState.WALK);
        }
        else
        {
            anim.SetInteger("curState", (int)curState);
        }
    }
    //private Oncoll
}

