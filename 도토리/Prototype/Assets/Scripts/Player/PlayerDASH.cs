using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDASH : PlayerFSMController
{
    [SerializeField]
    Vector3 deltaMove;

    float maxDashTime = 1.0f;
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

        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, deltaMove.normalized, PlayerController.BOXCAST_DISTANCE, controller.layerMask);

        if (groundHit)
        {
            if (groundHit.transform.gameObject.layer == 10)
            {
                deltaMove = Vector3.zero;
                controller.states[PlayerState.DASH].enabled = false;
            }
            else if(groundHit.transform.gameObject.layer==11)
            {
                if(!controller.isAirColliderPassingEnd)
                {
                    groundHit.transform.GetComponent<TilemapCollider2D>().enabled = false;
                }
            }
                
        }
        else
        {
            if(deltaMove!=Vector3.zero)
                transform.Translate(deltaMove * Time.deltaTime * controller.stat.dashSpeed);
        }

    }
}
