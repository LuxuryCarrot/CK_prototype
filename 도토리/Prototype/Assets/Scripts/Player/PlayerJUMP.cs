using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerJUMP : PlayerFSMController
{
    bool isJumping;
    int tileCheckCount;

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
        if (tileCheckCount == 0)
        {
            if (!controller.isAirColliderPassingEnd)
            {
                RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, transform.up, PlayerController.BOXCAST_DISTANCE, controller.layerMask);

                if (hit2D)
                {
                    if (hit2D.transform.gameObject.layer == 11)
                    {
                        hit2D.transform.GetComponent<TilemapCollider2D>().enabled = false;
                        tileCheckCount++;
                    }
                }
            }
        }
        if (transform.position.y >= transform.position.y + (controller.gravity) * Time.deltaTime)
        {
            isJumping = true;
        }
        if (isJumping)
        {
            if (!controller.isAirColliderPassingEnd)
            {
                //if (Physics2D.BoxCast(transform.position, new Vector2(0.4f, transform.lossyScale.y/PlayerController.HEIGHT_LENGTH), 0, -transform.up, PlayerController.BOXCAST_DISTANCE, controller.layerMask))
                if (Physics2D.Raycast(transform.position, Vector2.down, controller.playerCollider.bounds.extents.y+0.05f, controller.layerMask)) 
                {
                    if (controller.states[PlayerState.JUMP].enabled)
                        EffectManager.Instance.SetStateEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.JUMP - 1);

                    controller.jumpCount = 0;
                    tileCheckCount = 0;
                    controller.isGrounded = true;
                    controller.states[PlayerState.JUMP].enabled = false;
                }
            }
        }
    }

}
