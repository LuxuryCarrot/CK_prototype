using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void Move(float moveSpeed, float fallSpeed)
    {
        Vector2 moveDirection =  new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0);
    }


}
