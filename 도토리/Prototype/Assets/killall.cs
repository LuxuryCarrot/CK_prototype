using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killall : MonoBehaviour
{
    GameObject[] Monsters;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        Monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (Input.GetKey(KeyCode.P))
        {
            for (int i = 0; i < Monsters.Length; i++)
            {
                Destroy(Monsters[i]);
            }
        }
    }
}
