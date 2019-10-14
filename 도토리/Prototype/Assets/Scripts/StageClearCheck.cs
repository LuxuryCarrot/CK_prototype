using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearCheck : MonoBehaviour
{
    private int Mobcount;

    private GameObject[] Monsters;

    private void Awake()
    {
        //Monsters = GameObject.FindGameObjectsWithTag("Monster");

        
    }
    private void FixedUpdate()
    {
        Monsters = GameObject.FindGameObjectsWithTag("Monster");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*
            스테이지내의 모든 몬스터를 처치했는가?
            */
            if (Monsters.Length == 0)
            {
                if (!GameManager.Instance.asyncOper.allowSceneActivation)       //다음 씬을 비동기화로 불러왔다면
                {
                    if (SceneManager.GetActiveScene().buildIndex >= 3)      //마지막은 로딩하지 않는다
                    {
                        //GameManager.Instance.ui.bossUI.SetActive(true);
                        GameManager.Instance.asyncOper.allowSceneActivation = true;         //다음씬 불러오기
                    }
                    else
                    {
                        GameManager.Instance.asyncOper.allowSceneActivation = true;         //다음씬 불러오기
                        GameManager.Instance.StartCoroutine("LoadScene");
                    }
                }
            }





        }
    }
}
