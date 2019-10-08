using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    public static EffectManager Instance
    {
        get { return instance; }
    }

    public List<GameObject> particleStateObjs;
    public List<GameObject> particlePropertyObjs;
    public GameObject particlePlayerShotObj;

    public List<ParticleSystem> stateEffects = null;
    public List<ParticleSystem> propertyEffects = null;
    public ParticleSystem playerShotEffect = null;


    public List<Transform> posOfStateEffects;
    public Transform posOfPropertyEffects;
    public Transform posOfPlayerShotEffect;


    bool isFlipped;
    public bool isAttackEffectPlaying;

    private const float REFLECTION_POSITION_OF_ATTACK_UP = 2f;
    private const float REFLECTION_POSITION_OF_ATTACK_DOWN = 1.3f;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < particleStateObjs.Count; i++)
        {
            stateEffects.Add(Instantiate(particleStateObjs[i].GetComponent<ParticleSystem>(), transform.position, Quaternion.identity));
            stateEffects[i].transform.SetParent(posOfStateEffects[i]);
            stateEffects[i].Stop();
        }

        for (int i = 0; i < particlePropertyObjs.Count; i++)
        {
            propertyEffects.Add(Instantiate(particlePropertyObjs[i].GetComponent<ParticleSystem>(), transform.position, Quaternion.identity));
            propertyEffects[i].transform.SetParent(posOfPropertyEffects); //착지했을때의 위치를 그대로 사용한다
            propertyEffects[i].Stop();
        }

        playerShotEffect = Instantiate(particlePlayerShotObj.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
        playerShotEffect.transform.SetParent(posOfPlayerShotEffect);
        playerShotEffect.Stop();
    }

    private void Update()
    {
        if (stateEffects[2].isEmitting ||
            stateEffects[3].isEmitting)
        {
            isAttackEffectPlaying = true;                   //공격이펙트 재생중
        }
        else
        {
            isAttackEffectPlaying = false;
        }
    }

    public void SetStateEffect(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        SetStateEffectPostion(currentXPos, currentMouseXPos, newEffectOfCurState);

        if (!stateEffects[newEffectOfCurState].isPlaying)
        {
            //Debug.Log("effect play   name : " + effects[newEffectOfCurState].name);
            stateEffects[newEffectOfCurState].Play();
        }
    }

    public void SetElementalEffect(int curProperty)
    {
        if (!propertyEffects[curProperty].isPlaying)
        {
            propertyEffects[curProperty].transform.position = posOfPropertyEffects.position;
            propertyEffects[curProperty].Play();
        }
    }

    public void SetPlayerShotEffect()
    {
        if (!playerShotEffect.isPlaying)
        {
            playerShotEffect.transform.position = posOfPlayerShotEffect.position;
            playerShotEffect.Play();
        }
    }

    public void SetStateEffectPostion(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        switch (newEffectOfCurState)
        {
            case 0:                                                                                     //walk
                if (currentXPos > currentMouseXPos)
                {
                    stateEffects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);
                }
                stateEffects[newEffectOfCurState].transform.position = posOfStateEffects[newEffectOfCurState].position;
                break;
            case 1:                                                                                     //landing
                stateEffects[newEffectOfCurState].transform.position = posOfStateEffects[newEffectOfCurState].position;
                break;
            case 2:                                                                                     //attackUp
                if (currentXPos > currentMouseXPos)
                {
                    if (stateEffects[newEffectOfCurState].transform.localScale.x != -1)
                    {
                        stateEffects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = false;
                    }

                    if (!isFlipped)
                    {
                        Vector2 temp = posOfStateEffects[newEffectOfCurState].transform.position.normalized;
                        temp.y = stateEffects[newEffectOfCurState].transform.localPosition.y;

                        if (currentXPos < 0)
                            temp.x *= REFLECTION_POSITION_OF_ATTACK_UP;
                        else
                            temp.x *= -REFLECTION_POSITION_OF_ATTACK_UP;

                        stateEffects[newEffectOfCurState].transform.localPosition = new Vector2(temp.x, temp.y);
                    }
                }
                else
                {
                    if (stateEffects[newEffectOfCurState].transform.localScale.x != 1)
                    {
                        stateEffects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = true;
                    }

                    stateEffects[newEffectOfCurState].transform.position = posOfStateEffects[newEffectOfCurState].position;
                }
                break;
            case 3:                                                                                     //attackDown
                if (currentXPos > currentMouseXPos)
                {
                    if (stateEffects[newEffectOfCurState].transform.localScale.x != -1)
                    {
                        stateEffects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = false;
                    }

                    if (!isFlipped)
                    {
                        Vector2 temp = posOfStateEffects[newEffectOfCurState].transform.position.normalized;
                        temp.y = stateEffects[newEffectOfCurState].transform.localPosition.y;

                        if (currentXPos < 0)
                            temp.x *= REFLECTION_POSITION_OF_ATTACK_DOWN;
                        else
                            temp.x *= -REFLECTION_POSITION_OF_ATTACK_DOWN;

                        stateEffects[newEffectOfCurState].transform.localPosition = new Vector2(temp.x, temp.y);
                    }
                }
                else
                {
                    if (stateEffects[newEffectOfCurState].transform.localScale.x != 1)
                    {
                        stateEffects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = true;
                    }
                    stateEffects[newEffectOfCurState].transform.position = posOfStateEffects[newEffectOfCurState].position;
                }
                break;
            case 6:
                stateEffects[newEffectOfCurState - 2].transform.position = posOfStateEffects[newEffectOfCurState - 2].position;         //나중에 상태 순서에 맞게 다시 할것
                break;
        }
    }

    public void TargetStateEffectStop(int targetState)
    {
        stateEffects[targetState].Stop();
    }

    public void ClearStateEffects()
    {
        stateEffects.Clear();
    }
}
