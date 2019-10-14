using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundsController : MonoBehaviour
{
    public List<Transform> bgs;
    public List<float> xMoveSpeed;
    public List<float> yMoveSpeed;


    public GameObject camera;
    private Vector3 targetPosition;

    public SpriteRenderer bg;
    public BoxCollider2D bgBoundsCol;

    public Vector3 minColBound;
    public Vector3 maxColBound;

    public Vector3 prevPos;

    public float stHalfWidth;
    public float stHalfHeight;
    public bool isPrevPosSaved;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        minColBound = bgBoundsCol.bounds.min;
        maxColBound = bgBoundsCol.bounds.max;

        stHalfWidth = bg.sprite.bounds.size.x / 2f;          //반너비 
        stHalfHeight = bg.sprite.bounds.size.y / 2f;           //반높이
    }

    private void FixedUpdate()
    {
        if (camera != null)
        {
            if (!GameManager.Instance.isGamePause)
            {
                if (!camera.GetComponent<FollowPlayer>().isApproximate)
                {
                     StartCoroutine(this.PrevCameraPositionSave(camera.transform.position));

                    for (int i = 0; i < bgs.Count; i++)
                    {
                        if (prevPos.x < camera.transform.position.x) //카메라가 우측으로 이동
                        {
                            bgs[i].position = new Vector3(bgs[i].position.x + (-xMoveSpeed[i] * Time.deltaTime), bgs[i].position.y, bgs[i].position.z);
                        }
                        else
                        {
                            //좌측으로 이동
                            bgs[i].position = new Vector3(bgs[i].position.x + (xMoveSpeed[i] * Time.deltaTime), bgs[i].position.y, bgs[i].position.z);
                        }


                        if (prevPos.y < camera.transform.position.y)          //카메라가 위로 이동
                        {
                            bgs[i].position = new Vector3(bgs[i].position.x, bgs[i].position.y + (-yMoveSpeed[i] * Time.deltaTime), bgs[i].position.z);
                        }
                        else
                        {
                            //아래로 이동
                            bgs[i].position = new Vector3(bgs[i].position.x, bgs[i].position.y + (yMoveSpeed[i] * Time.deltaTime), bgs[i].position.z);
                        }

                        float clampedX = Mathf.Clamp(bgs[i].position.x, minColBound.x + stHalfWidth, maxColBound.x - stHalfWidth);
                        float clampedY = Mathf.Clamp(bgs[i].position.y, minColBound.y + stHalfHeight, maxColBound.y - stHalfHeight);

                        bgs[i].position = new Vector3(clampedX, clampedY, bgs[i].position.z);

                    }
                }
            } 
        }
    }


    IEnumerator PrevCameraPositionSave(Vector3 targetPos)
    {
        var tempPos = targetPos;
        if (!isPrevPosSaved)                      //포지션을 저장안했다면
        {
            tempPos = targetPos;                //저장
            isPrevPosSaved = true;
        }
        yield return new WaitForSeconds(0.1f);

        prevPos = tempPos;
        isPrevPosSaved = false;
    }
}
