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

        stage = 0;
        asyncOper = null;
    }

    private void Update()
    {
    }

    IEnumerator LoadScene()
    {
        switch (stage)
        {
            case 0:
                asyncOper = SceneManager.LoadSceneAsync("Play_Tutorial");
                break;
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

        ui.loadingSprite = GameObject.FindGameObjectWithTag("Loading");

        if (asyncOper != null)
        {
            //asyncOper.allowSceneActivation = false;
            while (!asyncOper.isDone)
            {
                ui.loadingSprite.SetActive(true);
                yield return null;
                Debug.Log(asyncOper.progress);
            }

            ui.loadingSprite.SetActive(false);

        }
        else
        {
            Debug.LogError("asyncOper null");
        }

    }

}
