using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDASH : PlayerFSMController
{
    public float dashDistance = 0.4f;
    public float dashDuration = 0.15f;

    bool isRayChecking = false;
    bool isDashing = false;
    int tileCheckCount;

    public override void BeginState()
    {
        base.BeginState();

        tileCheckCount = 0;
        controller.isGrounded = false;
        controller.dashDir = controller.mousePos - transform.position;
        controller.dashDir.z = 0;
    }

    private void Update()
    {
        if (tileCheckCount == 0)
        {
            if (!controller.isAirColliderPassingEnd)
            {
                RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, transform.lossyScale / 2, 0, controller.dashDir.normalized, 1, controller.layerMask);
                //RaycastHit2D groundHit = Physics2D.Raycast(transform.position, controller.dashDir.normalized, 1, controller.layerMask);

                if (groundHit)
                {
                    if (groundHit.transform.gameObject.layer == 11)
                    {
                        tileCheckCount++;
                        groundHit.transform.GetComponent<TilemapCollider2D>().enabled = false;
                        isRayChecking = true;
                    }
                    else if (groundHit.transform.gameObject.layer == 10)
                    {
                        tileCheckCount++;
                        isRayChecking = false;
                        controller.states[PlayerState.DASH].enabled = false;
                    }
                }
                else
                {
                    isRayChecking = true;
                }
            }
        }

        if (isRayChecking)
        {
            if (!isDashing)
            {
                if (controller.dashDir.magnitude >= 0.1f)
                {
                    this.StartCoroutine(this.DashRoutine(controller.dashDir.normalized));
                }
            }
        }
    }

    IEnumerator DashRoutine(Vector3 direction)
    {
        if (this.dashDistance <= 0.001f)
            yield break;

        if (this.dashDuration <= 0.001f)
        {
            this.transform.position += controller.dashDir * this.dashDistance;
            yield break;
        }

        this.isDashing = true;
        var elapsed = 0f;
        var start = this.transform.position;
        var target = this.transform.position + this.dashDistance * controller.dashDir;

        while (elapsed < this.dashDuration)
        {
            var iterTarget = Vector3.Lerp(start, target, elapsed / this.dashDuration);

            this.transform.position = iterTarget;

            yield return null;
            elapsed += Time.deltaTime;
        }


        this.transform.position = target;

        this.isRayChecking = false;
        this.isDashing = false;
        controller.states[PlayerState.DASH].enabled = false;
    }
}
