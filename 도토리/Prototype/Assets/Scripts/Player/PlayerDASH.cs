using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMController
{
    Vector3 dashDir;
    Vector3 deltaMove;

    public override void BeginState()
    {
        base.BeginState();
        dashDir = controller.mousePos.normalized * controller.dashSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, controller.lastMoveDir * controller.dashSpeed, Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, controller.lastMoveDir, controller.dashSpeed);

        deltaMove = Vector3.MoveTowards(transform.position, dashDir, controller.dashSpeed)-transform.position;

        if(deltaMove.sqrMagnitude<controller.dashSpeed*controller.dashSpeed)
        {
            controller.cc.Move(deltaMove * Time.deltaTime);
        }
        else
        {
        controller.states[PlayerState.DASH].enabled = false;
        }
    }
}
