using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loadingSprite;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartBtn()
    {
        GameManager.Instance.StartCoroutine("LoadScene");
    }
}
