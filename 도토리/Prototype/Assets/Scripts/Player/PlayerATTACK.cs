using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hit!");

        controller.states[PlayerState.ATTACK].enabled = false;
    }
}
