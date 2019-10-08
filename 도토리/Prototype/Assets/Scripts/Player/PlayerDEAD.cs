using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDEAD : PlayerFSMController
{
    public override void BeginState()
    {
        base.BeginState();

        if (EffectManager.Instance.stateEffects != null)
        {
            EffectManager.Instance.SetStateEffect(transform.position.x, controller.mousePos.x, (int)PlayerState.DEAD - 2);     //나중에 상태에 맞게 변경한다
        }
    }
}
