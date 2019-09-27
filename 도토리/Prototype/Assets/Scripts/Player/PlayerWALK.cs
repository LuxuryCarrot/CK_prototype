using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWALK : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            controller.lastMoveDir = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            controller.lastMoveDir = Vector3.right;
        }
        else
        {
            controller.states[PlayerState.WALK].enabled = false;
        }
        controller.cc.Move(controller.lastMoveDir * controller.stat.walkSpeed * Time.deltaTime);
        EffectManager.Instance.SetEffect(transform.position.x, controller.mousePos.x, (int)controller.stat.curState - 1);
    }
}
