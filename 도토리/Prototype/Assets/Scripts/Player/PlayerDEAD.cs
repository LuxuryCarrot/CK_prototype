using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDEAD : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();

        EffectManager.Instance.SetEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.DEAD-2);     //나중에 상태에 맞게 변경한다
        controller.states[PlayerState.DEAD].enabled = false;
    }
}
