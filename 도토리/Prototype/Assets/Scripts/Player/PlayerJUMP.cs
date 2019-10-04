using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJUMP : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();
        controller.isGroundCollisionCheck = false;
        controller.jumpCount++;
        controller.gravity = controller.stat.jumpForce;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
