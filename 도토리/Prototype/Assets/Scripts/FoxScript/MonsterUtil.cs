using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterUtil
{
    public static bool Detect(Transform monsterTr, Transform playerTr)
    {
        if ((monsterTr.position.x + 5.0f >= playerTr.position.x && monsterTr.position.x - 5.0f <= playerTr.position.x)
            && (monsterTr.position.x + 2.0f <= playerTr.position.x && monsterTr.position.x - 2.0f >= playerTr.position.x)
            && (monsterTr.position.y + 2.0f >= playerTr.position.y && monsterTr.position.y - 1.0f <= playerTr.position.y)) 
        {
            Debug.Log("check");
            return true;
        }
        else
        {
            return false;
        }
    }
}
