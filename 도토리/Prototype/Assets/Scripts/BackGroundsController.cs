using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundsController : MonoBehaviour
{
    public List<Transform> bgs;
    public List<float> moveSpeed;

    public GameObject camera;
    private Vector3 targetPosition;

    private void Start()
    {
        for(int i=0; i<bgs.Count; i++)
        {
            bgs[i].position = new Vector3(camera.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if(camera !=null)
        {
            for (int i = 0; i < bgs.Count; i++)
            {
                targetPosition.Set(camera.transform.position.x, this.transform.position.y, this.transform.position.z);

                bgs[i].position = Vector3.Lerp(bgs[i].position, targetPosition, moveSpeed[i] * Time.deltaTime);
            }
        }
    }
}
