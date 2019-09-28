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


    private void Awake()
    {
        instance = this;
        for (int i = 0; i < particleObjs.Count; i++)
        {
            effects.Add(Instantiate(particleObjs[i].GetComponent<ParticleSystem>(), transform.position, Quaternion.identity));
            effects[i].transform.SetParent(transform);
            effects[i].Stop();
        }

    }

    public void SetEffect(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].isPlaying)
                effects[i].Stop();
        }

        SetEffectPostion(currentXPos, currentMouseXPos, newEffectOfCurState);



        if (!effects[newEffectOfCurState].isPlaying)
        {
            Debug.Log("effect play");
            effects[newEffectOfCurState].Play();                
        }
    }

    public void SetEffectPostion(float currentXPos, float currentMouseXPos, int newEffectOfCurState)
    {
        switch (newEffectOfCurState)
        {
            case 0:                                             //walk
                if (currentXPos > currentMouseXPos)
                {
                    effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);
                }
                else
                    effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                break;
            case 1:                                            //landing
                effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                break;
            case 2:                                             //attack  
                if (currentXPos > currentMouseXPos)
                {
                    effects[newEffectOfCurState].transform.localScale *= new Vector2(-1, 1);
                }
                else
                    effects[newEffectOfCurState].transform.position = posOfEffects[newEffectOfCurState].position;
                break;
        }
    }
}
