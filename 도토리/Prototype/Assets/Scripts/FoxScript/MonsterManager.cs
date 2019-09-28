using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public MonsterFSMState manager;
    
    public virtual void BeginState()
    {

    }

    private void Awake()
    {
        manager = GetComponent<MonsterFSMState>();
    }
}
