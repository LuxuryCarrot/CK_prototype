using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public bool isPlayerDead = false;
    public int stage;
    private GameObject stageExit;
    public AsyncOperation asyncOper;
    public UIManager ui;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }

        stage = 1;
        asyncOper = null;
    }

    private void Start()
    {
        StartCoroutine(this.LoadScene());
    }

    public void PlayerHPGauge()
    {
        if (ui.playerHP.fillAmount <= 0)
        {
            ui.playerHP.fillAmount -= 6 / 100;
        }
        else
        {
            GameManager.instance.isPlayerDead = true;       //플레이어 사망
        }

    }

    IEnumerator LoadScene()
    {
        switch (stage)      //다음 씬 지정
        {
            case 1:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage2");
                break;
            case 2:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage3");
                break;
            case 3:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage4");
                break;
            case 4:
                asyncOper = SceneManager.LoadSceneAsync("Play_Boss_Stage");
                break;
        }

        stage++;                //스테이지 증가

        if (asyncOper != null)
        {
            asyncOper.allowSceneActivation = false;         
            while (!asyncOper.isDone)
            {
                yield return null;
                Debug.Log(asyncOper.progress);
            }
        }
        else
        {
            Debug.LogError("asyncOper null");
        }

    }

}
