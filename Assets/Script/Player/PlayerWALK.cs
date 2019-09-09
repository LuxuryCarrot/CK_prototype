using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWALK : PlayerFSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            manager.transform.Translate(new Vector2(manager.moveDirection.x, 0));
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            manager.transform.Translate(new Vector2(manager.moveDirection.x, 0));
        }
        if (manager.isMoving == false)
        {
            manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
