using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //public float smoothTimeX, smoothTimeY;
    //public Vector2 velocity;

    public GameObject target;
    private Vector3 targetPosition;
    public float moveSpeed;
    BoxCollider2D boundsCol;

    //맵의 최대 박스 컬라이더 영역
    private Vector3 minBound;
    private Vector3 maxBound;

    //카메라의 반너비,높이
    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player");
        boundsCol = GameObject.FindGameObjectWithTag("GroundBounds").GetComponent<BoxCollider2D>();
        minBound = boundsCol.bounds.min;
        maxBound = boundsCol.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;      //해상도

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
    }

    private void FixedUpdate()
    {
        //영역이 살짝 안맞음
        //float posX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothTimeX);
        //float posY = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothTimeY);

        //transform.position = new Vector3(posX, posY, transform.position.z);


        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.transform.position.y, this.transform.position.z);

            //1초에 moveSpeed만큼 이동
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }
}
