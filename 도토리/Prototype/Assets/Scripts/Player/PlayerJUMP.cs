using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJUMP : PlayerFSMController
{
    bool isJumping;

    public override void BeginState()
    {
        base.BeginState();
        isJumping = false;
        controller.jumpCount++;
        controller.gravity = controller.stat.jumpForce;
        controller.isGrounded = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= transform.position.y + (controller.gravity) * Time.deltaTime)
        {
            isJumping = true;
        }
        if (isJumping)
        {
            if (!controller.isAirColliderPassing)
            {

                if (Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, -transform.up, PlayerController.BOXCAST_DISTANCE, controller.layerMask))
                {
                    if (controller.states[PlayerState.JUMP].enabled)
                        EffectManager.Instance.SetEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.JUMP - 1);

                    controller.jumpCount = 0;
                    controller.states[PlayerState.JUMP].enabled = false;
                    controller.isGrounded = true;
                }
            }
        }
    }
}
