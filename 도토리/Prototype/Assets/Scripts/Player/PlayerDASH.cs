using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMController
{
    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, controller.lastMoveDir * controller.dashSpeed, Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, controller.lastMoveDir, controller.dashSpeed);
        controller.cc.Move(controller.dashDir * controller.dashSpeed);

        controller.states[PlayerState.DASH].enabled = false;
    }
}
