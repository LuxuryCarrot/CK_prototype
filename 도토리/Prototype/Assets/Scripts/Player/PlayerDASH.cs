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
        currentDashTime = 0;
        deltaMove = controller.dashDir;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDashTime < maxDashTime)
        {
            //최종 좌표는 이미 정해짐
            //deltaMove = controller.dashDir * controller.dashForce;

            deltaMove = deltaMove * controller.dashForce;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            deltaMove = Vector3.zero;
            controller.states[PlayerState.DASH].enabled = false;
        }

        if (controller.sprite.flipX)
        {
            if (deltaMove.x > 0)
                deltaMove.x = -deltaMove.x;
        }
        else
        {
            if (deltaMove.x < 0)
                deltaMove.x = -deltaMove.x;
        }

        controller.cc.Move(deltaMove * Time.deltaTime * controller.dashSpeed);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerDASH : PlayerFSMController
//{
//    Vector3 dashDir;

//    const float maxDashTime = 1.0f;
//    float dashStoppingSpeed = 0.1f;
//    float currentDashTime;

//    public override void BeginState()
//    {
//        base.BeginState();
//        currentDashTime = 0;
//        dashDir = controller.mousePos;
//        dashDir.Normalize();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (currentDashTime < maxDashTime)
//        {
//            dashDir = dashDir * controller.dashForce;
//            currentDashTime += dashStoppingSpeed;
//        }
//        else
//        {
//            dashDir = Vector3.zero;
//            controller.states[PlayerState.DASH].enabled = false;
//        }

//        if(controller.sprite.flipX)
//        {
//            if(dashDir.x > 0)
//                dashDir.x = -dashDir.x;
//        }
//        else
//        {
//            if(dashDir.x < 0)
//                dashDir.x = -dashDir.x;
//        }

//        controller.cc.Move(dashDir * Time.deltaTime * controller.dashSpeed);
//    }
//}
