using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            /*
            스테이지내의 모든 몬스터를 처치했는가?
            */

            if (!GameManager.Instance.asyncOper.allowSceneActivation)       //다음 씬을 비동기화로 불러왔다면
            {
                GameManager.Instance.asyncOper.allowSceneActivation = true;         //다음씬 불러오기
                GameManager.Instance.StartCoroutine("LoadScene");
            }




        }
    }
}
