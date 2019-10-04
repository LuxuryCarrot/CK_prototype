using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDOWN : PlayerFSMController
{
    public bool isThereGround;
    public bool isPlaformSearching;
    public bool isPlaformChecking;

    List<TilemapCollider2D> plaformColliders = new List<TilemapCollider2D>();
    float platformDisabledTime;
    float disabledMaxTime = 1.5f;

    Vector3 groundPos;

    int airPlaformCount;
    int tempCount;

    public override void BeginState()
    {
        base.BeginState();
        //groundPos = Vector3.zero;
        isPlaformSearching = true;
        isPlaformChecking = false;
        isThereGround = false;
        platformDisabledTime = 0;
        airPlaformCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaformSearching)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.lossyScale / 2.5f, 0, -transform.up, 20f, controller.layerMask);
            if (hit)
            {
                if (hit.transform.gameObject.layer == 11)           //only airGround
                {
                    plaformColliders.Add(hit.transform.GetComponent<TilemapCollider2D>());

                    plaformColliders[plaformColliders.Count - 1].enabled = false;

                    airPlaformCount++;
                }
                else if (hit.transform.gameObject.layer == 10)      //ground 발견
                {
                    isThereGround = true;
                    isPlaformSearching = false;
                    isPlaformChecking = true;
                    //groundPos = hit.transform.position;
                }
            }
            else
            {
                isPlaformSearching = false;
                isPlaformChecking = true;
            }
        }

        if (!isPlaformSearching && isPlaformChecking)
        {
            if (plaformColliders.Count != 0)
            {
                for (int i = 0; i < plaformColliders.Count; i++)
                {
                    plaformColliders[i].enabled = true;
                }
            }

            if (!isThereGround && plaformColliders.Count == 0)                                 //ground, airPlaform Not Find               
            {
                controller.states[PlayerState.DOWN].enabled = false;
            }

            if (isThereGround && plaformColliders.Count == 0)                                //ground Find airPlaform Not Find 
            {
                controller.isFalling = false;
                controller.states[PlayerState.DOWN].enabled = false;
            }

            if (!isThereGround && plaformColliders.Count != 0)                            //ground Not Find  airPlaform Find
            {
                switch (airPlaformCount)
                {
                    case 1:                                                         //airPlaform Find  but ground Not Find... PlayerDown end
                        controller.states[PlayerState.DOWN].enabled = false;
                        break;
                    default:
                        Physics2D.IgnoreCollision(controller.playerCollider, plaformColliders[0]);     //ground Not Find but Two airPlaform Find... 
                        controller.isFalling = true;
                        tempCount = airPlaformCount;
                        airPlaformCount = 0;
                        isPlaformChecking = false;
                        break;
                }
            }

            if (isThereGround && plaformColliders.Count != 0)                          //ground, airPlaform Find
            {
                Physics2D.IgnoreCollision(controller.playerCollider, plaformColliders[0]);
                controller.isFalling = true;
                tempCount = airPlaformCount;
                airPlaformCount = 0;
                isPlaformChecking = false;
            }

        }

        if (tempCount != 0 && !isPlaformChecking)
        {
            //if (plaformColliders.Count > 1)
            //{
            //    Vector3 Dis = plaformColliders[1].transform.position - plaformColliders[0].transform.position;
            //    disabledMaxTime = Dis.y;
            //}
            //else
            //{
            //    disabledMaxTime = 3f;
            //}

            platformDisabledTime += Time.deltaTime;

            Debug.Log("Falling distance : " + disabledMaxTime);
            Debug.Log("Falling Pow distance : " + Mathf.Pow(disabledMaxTime, 2));
            Debug.Log("Falling Pow distance Time : " + Mathf.Pow(disabledMaxTime, 2)* 0.1f);


            if (platformDisabledTime >= Mathf.Pow(disabledMaxTime, 2) * 0.1f)
            {
                controller.isFalling = false;
                Physics2D.IgnoreCollision(controller.playerCollider, plaformColliders[0], false);
                plaformColliders.Clear();
                controller.states[PlayerState.DOWN].enabled = false;
            }
        }
    }
}
