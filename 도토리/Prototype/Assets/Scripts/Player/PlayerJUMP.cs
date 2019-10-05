using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJUMP : PlayerFSMController
{
    bool isJumping;

    //const float MAX_COLLISION_CHECK_TIME = 0.1f;

    //[SerializeField]
    //float currentCollisionCheckTime;
    //float checkStoppingSpeed = 0.04f;

    public override void BeginState()
    {
        base.BeginState();
        isJumping = false;
        //currentCollisionCheckTime = 0;
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
            Debug.Log("하락 시작!");
        }
        if (isJumping && !controller.isAirColliderPassing)
        {
            if (Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, -transform.up, PlayerController.BOXCAST_DISTANCE, controller.layerMask))
            {
                //currentCollisionCheckTime += checkStoppingSpeed;

                //if (currentCollisionCheckTime>=MAX_COLLISION_CHECK_TIME)
                //{
                    Debug.Log("바닥 발견!");
                    if (controller.states[PlayerState.JUMP].enabled)
                        EffectManager.Instance.SetEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.JUMP - 1);

                    controller.jumpCount = 0;
                    controller.states[PlayerState.JUMP].enabled = false;
                    controller.isGrounded = true;
                //}
            }
        }
    }
}
