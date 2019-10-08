using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSHOT : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();
        controller.curShotAnimSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.curShotAnimSpeed >= PlayerController.MAX_SHOT_ANIM_TIME)
        {
            controller.curShotAnimSpeed = 0;
            controller.states[PlayerState.SHOT].enabled = false;
        }
        else
        {
            controller.curShotAnimSpeed += Time.deltaTime;
        }
    }
}
