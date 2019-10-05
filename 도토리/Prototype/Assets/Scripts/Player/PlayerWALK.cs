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

        RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, controller.lastMoveDir, PlayerController.BOXCAST_DISTANCE, controller.layerMask);

        if (hit2D)
        {
            if (hit2D.transform.gameObject.layer == 10)
                controller.states[PlayerState.WALK].enabled = false;
        }
        else
        {
            transform.Translate(controller.lastMoveDir * controller.stat.walkSpeed * Time.deltaTime);
        }
    }
}
