using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHand : MonoBehaviour
{
    private GameObject Player;

    public float RotationSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DestroyArm", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        if (transform.rotation.z == -51)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PlayerAttacked");
            Player.SendMessage("ApllyDamage", 1.0f);
        }
    }
    void DestroyArm()
    {
        Destroy(gameObject);
    }

}
