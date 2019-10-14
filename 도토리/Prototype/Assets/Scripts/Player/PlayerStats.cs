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
    public float changeWalkSpeed;
    public float normalWalkSpeed;
    public float fallSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashForce;

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
        normalWalkSpeed = walkSpeed;
        changeWalkSpeed = 1.5f;
        dashSpeed = 4f;
        dashForce = 1.2f;
        fallSpeed = 3.5f;
        weaponDamage = 10f;
    }
}
