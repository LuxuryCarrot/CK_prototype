using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMController
{
    Vector3 dashDir;
    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, controller.lastMoveDir * controller.dashSpeed, Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, controller.lastMoveDir, controller.dashSpeed);

        dashDir = controller.mousePos.normalized * controller.dashSpeed;
        controller.cc.Move(dashDir);

        controller.states[PlayerState.DASH].enabled = false;
    }
}
