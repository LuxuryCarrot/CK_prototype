using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private bool isKeyInputting = false;

    public bool isUpAttacked = true;

    public float attackDir = 0;

    private float gravity;
    public float verticalVelocity;

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
    public WeaponSword weapon;

    public Transform spriteTrans;
    public Vector3 flipScale;

    public ElementalProperty curWeaponProperty;

    public float curElementalDurantionTime;
    public float maxElementalDurantionTime;     //const

    public Dictionary<PlayerState, PlayerFSMController> states =
        new Dictionary<PlayerState, PlayerFSMController>();


    private void Awake()
    {
        stat = GetComponent<PlayerStats>();

        verticalVelocity = 9.8f;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        spriteTrans = transform.GetChild(0);
        flipScale = spriteTrans.localScale;

        monster = GameObject.FindGameObjectWithTag("Fox").transform;
        weapon = new WeaponSword();
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
        SetState(stat.startState);
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
            spriteTrans.localScale = new Vector3(-flipScale.x, flipScale.y, flipScale.z);
        }
        else
        {
            spriteTrans.localScale = new Vector3(flipScale.x, flipScale.y, flipScale.z);
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
            anim.SetInteger("curState", (int)stat.curState);

        if (states[PlayerState.ATTACK].enabled)
        {
            anim.SetFloat("attackDir", attackDir);
        }
    }

    public void Gravity()
    {
        if (cc.isGrounded)
        {
            gravity = -stat.fallSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.W))
            {
                gravity = stat.jumpForce;
                SetState(PlayerState.JUMP);
            }
        }
        else
        {
            gravity -= stat.fallSpeed * Time.deltaTime;
        }
        moveDirection = Vector2.zero;
        moveDirection.y = gravity * verticalVelocity;
        cc.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.gameObject.tag == "Ground") &&
            (gravity <= stat.fallSpeed))
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
        stat.currentHP -= damage;

        if (stat.currentHP <= 0)
        {
            Debug.Log("PlayerDead");
        }
    }
}
