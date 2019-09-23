using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMController
{
    [SerializeField]
    Vector3 deltaMove;

    const float maxDashTime = 1.0f;
    float dashStoppingSpeed = 0.1f;
    float currentDashTime;

    public override void BeginState()
    {
        base.BeginState();

        controller.dashDir = controller.mousePos - transform.position;
        controller.dashDir.Normalize();

        currentDashTime = 0;
        deltaMove = controller.dashDir;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDashTime < maxDashTime)
        {
            deltaMove = deltaMove * controller.stat.dashForce;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            deltaMove = Vector3.zero;
            controller.states[PlayerState.DASH].enabled = false;
        }

        if (controller.spriteTrans.localScale.x <= 0)           
        {
            if (deltaMove.x > 0)
                deltaMove.x = -deltaMove.x;
        }
        else
        {
            if (deltaMove.x < 0)
                deltaMove.x = -deltaMove.x;
        }

        controller.cc.Move(deltaMove * Time.deltaTime * controller.stat.dashSpeed);
    }
}
