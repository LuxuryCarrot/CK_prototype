using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSMController : MonoBehaviour
{
    public MonsterController Mcontroller;

    public virtual void BeginState()
    {

    }
    void Update()
    {
        Mcontroller = GetComponent<MonsterController>();   
    }
}
