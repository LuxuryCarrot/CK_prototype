using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerATTACK : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.curAttackAnimSpeed >= controller.maxAttackAnimSpeed)
        {
            if (controller.attackDir == 1)                                                                     //attack up
                EffectManager.Instance.SetStateEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.ATTACK - 1);
            else if (controller.attackDir == -1)                                                                                    //attack down
                EffectManager.Instance.SetStateEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.ATTACK);

            controller.curAttackAnimSpeed = 0;
            controller.AttackDirCheck(0);
            controller.states[PlayerState.ATTACK].enabled = false;
        }
        else
        {
            controller.curAttackAnimSpeed += Time.deltaTime;
        }
    }
}

