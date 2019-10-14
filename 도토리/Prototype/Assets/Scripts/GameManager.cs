using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public bool isPlayerDead = false;
    public bool isGameClear = false;
    public bool isGamePause = false;
    public bool isItemEatting;
    public int stage;
    public int bossKillCount;
    private GameObject stageExit;
    public AsyncOperation asyncOper;

    public UIManager ui;
    public FollowPlayer camera;
    public PlayerController player;
    public BossMonster bossMonster;

    public string prevStageName;

    public float damageToPlayerHp;

    private void Awake()
    {
        SceneManager.LoadScene("Cut_Scene", LoadSceneMode.Additive);

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
        damageToPlayerHp = Mathf.CeilToInt((100 / 6) + 1) / 100f;
        StartCoroutine(this.LoadScene());
    }

    private void Update()
    {
        if (ui.playerHP.fillAmount == 0)        //도토리가 없으면 플레이어 사망
        {
            isPlayerDead = true;
        }

        //if (ui.bossHP.fillAmount == 0)             //보스의 Hp가 0가 되면 게임 클리어
        //{
        //    isGameClear = true;
        //}

        if(isGameClear)
        {
            GameClear();
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Cut_Scene")
        {
            //스테이지 전환시 카메라 다시 셋팅
            SetStage();
            SetCamera();
        }

        Debug.Log(scene.name);
    }

    public void SetCamera()
    {
        if (camera != null)
        {
            camera.boundsCol = GameObject.FindGameObjectWithTag("GroundBounds").GetComponent<BoxCollider2D>();
            camera.minBound = camera.boundsCol.bounds.min;
            camera.maxBound = camera.boundsCol.bounds.max;
            camera.halfHeight = camera.theCamera.orthographicSize;
            camera.halfWidth = camera.halfHeight * Screen.width / Screen.height;      //해상도
        }
        else
            Debug.LogError("camera need");
    }

    public void SetStage()
    {
        if (player != null)
        {
            player.transform.position = GameObject.FindGameObjectWithTag("StartPosition").transform.position;
            player.tileMaplCollider = GameObject.FindGameObjectWithTag("Air").transform.GetComponent<TilemapCollider2D>();
        }
        else
            Debug.LogError("Player need");
    }

    public void SetPlayer()
    {
        player.states[PlayerState.DEAD].enabled = false;
        player.SetState(PlayerState.IDLE);
        player.stat.finalDamage = player.stat.weaponDamage;
        player.stat.curWeaponProperty = ElementalProperty.None;
    }

    public void PlayerHPGauge()
    {
        if (ui.playerHP.fillAmount > 0)         // 도토리가 있으면 데미지
        {
            ui.playerHP.fillAmount -= damageToPlayerHp;
        }
    }

    public void BossHPGauge()
    {
        if(ui.bossHP.fillAmount > 0)
        {
            ui.bossHP.fillAmount -= player.stat.finalDamage / 100f;
        }
    }

    public void GameClear()
    {
        isGameClear = false;
        bossKillCount++;
        SceneManager.LoadScene("Cut_Scene", LoadSceneMode.Additive);
    }

    IEnumerator LoadScene()
    {
        switch (stage)      //다음 씬 지정
        {
            case 1:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage2");
                stage++;                //스테이지 증가
                break;
            case 2:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage3");
                stage++;                //스테이지 증가
                break;
            case 3:
                asyncOper = SceneManager.LoadSceneAsync("Play_Stage4");
                stage++;                //스테이지 증가
                break;
            case 4:
                asyncOper = SceneManager.LoadSceneAsync("Play_Boss_Stage");
                break;
        }


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
