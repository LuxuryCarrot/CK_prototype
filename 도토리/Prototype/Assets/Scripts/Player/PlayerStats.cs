using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE = 0,
    WALK,
    JUMP,
    ATTACK,
    DASH,
    DOWN,
    DEAD,       
    SHOT        //피격
}

public enum ElementalProperty
{
    None,
    Fire,
    Water,
    Grass
}

public class PlayerStats : MonoBehaviour
{
    public float walkSpeed;
    public float fallSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashForce;

    public float hp;
    public float currentHP;

    public float weaponDamage;
    public float finalDamage;




    public PlayerController controller;

    public PlayerState startState;
    public PlayerState curState;

    public ElementalProperty curWeaponProperty;


    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        jumpForce = 1.5f;
        walkSpeed = 3f;
        dashSpeed = 6f;
        dashForce = 1.2f;
        fallSpeed = 4f;
        hp = 10;
        weaponDamage = 10f;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
    }
}
