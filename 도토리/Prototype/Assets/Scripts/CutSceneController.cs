using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public GameObject tuto;
    public GameObject end;

    public Animator cutAnim;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.bossKillCount == 1)
        {
            if (SceneManager.GetActiveScene().isLoaded)      //씬의 로딩이 완료됐다면
            {
                EndCutSceneStart();
            }
        }
    }

    public void SkipButton()
    {
        GameManager.Instance.isGamePause = false;
        StartCoroutine(this.UnloadScene());
    }

    void EndCutSceneStart()
    {
        GameManager.Instance.bossKillCount = 0;

        if (tuto.activeSelf)
            tuto.SetActive(false);
        if (!end.activeSelf)
            end.SetActive(true);

        cutAnim.SetTrigger("EndTrigger");
    }

    public void EndCutSceneFinish()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            Application.OpenURL("http://google.com");
        #else
            Application.Quit();
        #endif
    }

    public void TutoCutSceneStart()
    {
        GameManager.Instance.isGamePause = true;
    }

    public void TutoCutSceneEnd()
    {
        GameManager.Instance.isGamePause = false;
        StartCoroutine(this.UnloadScene());          //it might be worth trying to defer the loading and unloading and not do it inside a callback 
    }

    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadScene("Cut_Scene");
    }
}
