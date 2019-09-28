using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterState
{
    IDLE = 0,
    MOVE,
    ATTACK,
    CHASE,
    DEAD
}
public class MonsterFSMState : MonoBehaviour
{
    public MonsterState currentState;
    public MonsterState startState;
    public Animator anim;
    public MonsterStat stat;

    public GameObject Player;

    public Dictionary<MonsterState, MonsterManager> states
        = new Dictionary<MonsterState, MonsterManager>();
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();

        stat = GetComponent<MonsterStat>();

        Player = GameObject.FindGameObjectWithTag("Player");

        states.Add(MonsterState.IDLE, GetComponent<MonsterIDLE>());
        states.Add(MonsterState.ATTACK, GetComponent<MonsterATTACK>());
        states.Add(MonsterState.CHASE, GetComponent<MonsterCHASE>());
        states.Add(MonsterState.DEAD, GetComponent<MonsterDEAD>());
        states.Add(MonsterState.MOVE, GetComponent<MonsterMOVE>());

    }
    private void Start()
    {
        SetState(startState);
    }
    public void SetState(MonsterState newState)
    {
        if (currentState == MonsterState.DEAD)
        {
            return;
        }

        foreach (MonsterManager state in states.Values)
        {
            state.enabled = false;
        }

        currentState = newState;
        states[currentState].enabled = true;
        states[currentState].BeginState();
        anim.SetInteger("CurrentState", (int)currentState);
    }

    public void SetDead()
    {
        SetState(MonsterState.DEAD);   
    }
}
