using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSMController : MonoBehaviour
{
    public PlayerController controller;

    public virtual void BeginState()
    {

    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
}
