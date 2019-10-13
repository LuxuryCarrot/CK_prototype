using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    GameObject PausePanel;
    private void Awake()
    {
        PausePanel = GameObject.FindGameObjectWithTag("PausePanel");
    }

    //private void Start()
    //{
    //    PausePanel.SetActive(false);
    //}
    //public void OnClickPause()
    //{
    //    PausePanel.SetActive(true);
    //}
    //public void OnClickContinue()
    //{
    //    PausePanel.SetActive(false);
    //}
    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL("http://google.com");
#else
        Application.Quit();
#endif
    }
}
