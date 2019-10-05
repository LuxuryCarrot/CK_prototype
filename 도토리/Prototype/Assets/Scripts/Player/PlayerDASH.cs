using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMController
{
    [SerializeField]
    Vector3 deltaMove;

    float maxDashTime = 0.9f;
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

    private void FixedUpdate()
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

        RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, deltaMove.normalized, PlayerController.BOXCAST_DISTANCE, controller.layerMask);

        if (hit2D)
        {
            if (hit2D.transform.gameObject.layer == 10)
            {
                controller.states[PlayerState.DASH].enabled = false;
            }
        }
        else
        {
            transform.Translate(deltaMove * Time.deltaTime * controller.stat.dashSpeed);
        }
    }

}
