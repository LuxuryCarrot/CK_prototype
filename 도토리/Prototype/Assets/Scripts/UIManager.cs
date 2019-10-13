using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public Image playerHP;
    public Image bossHP;
    public GameObject canvas;
    public GameObject prefab_floating_text;
    public GameObject playerShotAnimUI;
    public GameObject gameOverUI;
    public GameObject pauseGrayUI;
    public GameObject bossUI;
    //public GameObject pauseButtonUI;
    //public SpriteState pauseButtonSt;
    Animator shotAnimUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        shotAnimUI = playerShotAnimUI.GetComponent<Animator>();
        //pauseButtonSt = new SpriteState();
    }

    private void Update()
    {
        if (playerShotAnimUI.activeSelf)
        {
            if (shotAnimUI.GetCurrentAnimatorStateInfo(0).IsName("UI_Shot") &&       //해당 애니메이션이 종료되었는지 체크
                shotAnimUI.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                playerShotAnimUI.SetActive(false);
            }
        }

        if (GameManager.Instance.isPlayerDead)       //사망하면 gameOverUI 활성화
        {
            gameOverUI.SetActive(true);
        }
    }
    
    public void ReStartButton()
    {
        playerHP.fillAmount = 1;
        GameManager.Instance.isPlayerDead = false;
        GameManager.Instance.SetStage();
        GameManager.Instance.SetPlayer();

        gameOverUI.SetActive(false);
    }

    public void PauseButton()
    {
        //var hoverSprite = pauseButtonUI.GetComponent<Button>().spriteState.highlightedSprite;
        //pauseButtonSt.highlightedSprite = hoverSprite;
        //pauseButtonUI.GetComponent<Button>().spriteState = pauseButtonSt;

        EventSystem.current.SetSelectedGameObject(null);
        pauseGrayUI.SetActive(true);
    }

    public void PauseExcuteButton()
    {
        pauseGrayUI.SetActive(false);
    }

    public void ExitButton()
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
