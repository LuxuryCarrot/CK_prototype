using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public float HP;
    public float currentHP;

    private int PatternCount;
    private int PatternFlag;
    private int AnimState;

    private bool isPatternA;
    private bool isPatternB;
    private bool isPatternC;

    private bool Phase0;
    private bool Phase1;
    private bool Phase2;
    private bool Phase3;
    private bool isDead;

    public Image BossHP_UI;

    public GameObject FoxPreFab;
    public GameObject FrogPreFab;
    public GameObject RaccoonPreFab;

    private GameObject BossStartSprite;

    public Transform summonPos;

    public GameObject Arm;
    public Transform ArmPos;

    private Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        BossStartSprite = transform.GetChild(0).gameObject;
        Destroy(BossStartSprite);

        currentHP = HP;
        PatternFlag = 0;

        Phase0 = true;
        Phase1 = false;
        Phase2 = false;
        Phase3 = false;

        isPatternA = false;
        isPatternB = false;
        isPatternC = false;
        StartCoroutine("ChangePattern");
    }


    // Update is called once per frame
    void Update()
    {
        Pattern();
        if (!isDead)
        {
            if (currentHP < 600.0f && currentHP > 399.0f)
            {
                Phase1 = true;
            }
            else if (currentHP < 400.0f && currentHP > 199.0f)
            {
                Phase1 = false;
                Phase2 = true;
            }
            else if (currentHP < 200.0f && currentHP > 0.0f)
            {
                Phase2 = false;
                Phase3 = true;
            }

            if (PatternFlag > -1 && PatternFlag < 6)
            {
                PatternCount += 1;
                isPatternA = true;
            }
            else if (PatternFlag > 5 && PatternFlag  < 8)
            {
                PatternCount += 1;
                isPatternB = true;
            }
            else if (PatternFlag > 7 && PatternFlag < 10)
            {
                PatternCount += 1;
                isPatternC = true;
            }
        }
        else if(isDead)
        {

        }
    }

    IEnumerator ChangePattern()
    {
        yield return new WaitForSeconds(3.0f);
        PatternFlag = Random.Range(0, 10);
        PatternCount = 0;
        StartCoroutine("ChangePattern");
    }

    IEnumerator DestroyMonster()
    {

        yield return new WaitForSeconds(1.0f);
    }

    void Pattern()
    {
        if (isPatternA)
        {
            if (PatternCount == 1)
            {
                Instantiate(Arm, ArmPos.position, ArmPos.rotation);
                Debug.Log("isPatternA");
            }
            isPatternA = false;
        }
        else if (isPatternB)
        {
            if (PatternCount == 1)
            {
                 Debug.Log("isPatternB");
            }
            isPatternB = false;
        }
        else if (isPatternC)
        {
            if (PatternCount == 1)
            {
                if (Phase0)
                {
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                }
                else if (Phase1)
                {
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                }
                else if (Phase2)
                {
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                }
                else if (Phase3)
                {
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(RaccoonPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FoxPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);

                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                    Instantiate(FrogPreFab, (new Vector3(summonPos.position.x + Random.Range(-13.0f, 13.0f), summonPos.position.y)), summonPos.rotation);
                }
                
                Debug.Log("isPatternC");
            }
            isPatternC = false;
        }

    }
    void ApplyDamage(float damage)
    {
        currentHP -= damage;

        BossHP_UI.fillAmount -= damage / HP;

        if (currentHP <= 0)
        {
            isDead = true;
        }
    }
}
