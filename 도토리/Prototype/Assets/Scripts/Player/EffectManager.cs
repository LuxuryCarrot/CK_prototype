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

    public List<GameObject> particleObjs;

    public List<ParticleSystem> effects = null;
    public List<Transform> posOfEffects;

    bool isFlipped;

    private const float REFLECTION_POSITION_OF_ATTACK_UP = 2f;
    private const float REFLECTION_POSITION_OF_ATTACK_DOWN = 1.3f;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < particleObjs.Count; i++)
        {
            effects.Add(Instantiate(particleObjs[i].GetComponent<ParticleSystem>(), transform.position, Quaternion.identity));
            effects[i].transform.SetParent(posOfEffects[i]);
            effects[i].Stop();
        }

    }

    public void SetEffect(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        SetEffectPostion(currentXPos, currentMouseXPos, newEffectOfCurState);

        if (!effects[newEffectOfCurState].isPlaying)
        {
            //Debug.Log("effect play   name : " + effects[newEffectOfCurState].name);
            effects[newEffectOfCurState].Play();
        }
    }

    public void SetEffectPostion(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        switch (newEffectOfCurState)
        {
            case 0:                                                                                     //walk
                if (currentXPos > currentMouseXPos)
                {
                    effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);
                }
                effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                break;
            case 1:                                                                                     //landing
                effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                break;
            case 2:                                                                                     //attackUp
                if (currentXPos > currentMouseXPos)
                {
                    if (effects[newEffectOfCurState].transform.localScale.x != -1)
                    {
                        effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = false;
                    }

                    if (!isFlipped)
                    {
                        Vector2 temp = posOfEffects[newEffectOfCurState].transform.position.normalized;
                        temp.y = effects[newEffectOfCurState].transform.localPosition.y;

                        if (currentXPos < 0)
                            temp.x *= REFLECTION_POSITION_OF_ATTACK_UP;
                        else
                            temp.x *= -REFLECTION_POSITION_OF_ATTACK_UP;

                        effects[newEffectOfCurState].transform.localPosition = new Vector2(temp.x, temp.y);
                    }
                }
                else
                {
                    if (effects[newEffectOfCurState].transform.localScale.x != 1)
                    {
                        effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = true;
                    }

                    effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                }
                break;
            case 3:                                                                                     //attackDown
                if (currentXPos > currentMouseXPos)
                {
                    if (effects[newEffectOfCurState].transform.localScale.x != -1)
                    {
                        effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = false;
                    }

                    if (!isFlipped)
                    {
                        Vector2 temp = posOfEffects[newEffectOfCurState].transform.position.normalized;
                        temp.y = effects[newEffectOfCurState].transform.localPosition.y;

                        if (currentXPos < 0)
                            temp.x *= REFLECTION_POSITION_OF_ATTACK_DOWN;
                        else
                            temp.x *= -REFLECTION_POSITION_OF_ATTACK_DOWN;

                        effects[newEffectOfCurState].transform.localPosition = new Vector2(temp.x, temp.y);
                    }
                }
                else
                {
                    if (effects[newEffectOfCurState].transform.localScale.x != 1)
                    {
                        effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);        //effect flip

                        isFlipped = true;
                    }
                    effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                }
                break;
        }
    }
}
