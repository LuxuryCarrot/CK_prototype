using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMOVE : MonsterManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MonsterUtil.Detect(manager.transform, manager.Player.transform))
        {
            manager.SetState(MonsterState.CHASE);
            return;
        }
    }
}
