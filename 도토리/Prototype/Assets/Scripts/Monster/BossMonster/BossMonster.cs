using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public float HP;
    private float currentHP;

    private int PatternFlag;

    private bool isPatternA;
    private bool isPatternB;
    private bool isPatternC;

    public GameObject Arm;
    public Transform ArmPos;

    // Start is called before the first frame update
    void Awake()
    {
        currentHP = HP;
        PatternFlag = 0;

        isPatternA = false;
        isPatternB = false;
        isPatternC = false;
        StartCoroutine("ChangePattern");
    }

    // Update is called once per frame
    void Update()
    {
        Pattern();
        if (PatternFlag > -1 && PatternFlag < 6)
        {
            isPatternA = true;
        }
        else if (PatternFlag > 5 && PatternFlag  < 8)
        {
            isPatternB = true;
        }
        else if (PatternFlag > 7 && PatternFlag < 10)
        {
            isPatternC = true;
        }
    }

    IEnumerator ChangePattern()
    {
        PatternFlag = Random.Range(0, 10);

        yield return new WaitForSeconds(3.0f);
        StartCoroutine("ChangePattern");
    }

    void Pattern()
    {
        if (isPatternA)
        {
            Debug.Log("isPatternA");
            isPatternA = false;
        }
        else if (isPatternB)
        {
            Debug.Log("isPatternB");
            isPatternB = false;
        }
        else if (isPatternC)
        {
            Debug.Log("isPatternC");
            isPatternC = false;
        }
    }
}
