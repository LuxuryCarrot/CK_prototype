using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCheck : MonoBehaviour
{
    PlayerController player;
    TilemapCollider2D tilemapCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        tilemapCollider = transform.parent.GetComponent<TilemapCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!tilemapCollider.enabled && !player.isAirColliderPassing)
            {
                //Debug.Log("PassingStart");
                player.isAirColliderPassing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (player.isAirColliderPassing)
            {
                //Debug.Log("PassingEnd");
                player.isAirColliderPassing = false;
                player.isAirColliderPassingEnd = true;
            }
        }
    }

    private void Update()
    {
        if (player.isAirColliderPassingEnd && !tilemapCollider.enabled && !player.isAirColliderPassing)
        {
            tilemapCollider.enabled = true;
            player.isAirColliderPassingEnd = false;
        }
    }
}
