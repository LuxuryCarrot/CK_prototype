using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AirCollision : MonoBehaviour
{
    public Tilemap tileMap = null;

    public List<Vector3> availablePlaces;

    TilemapCollider2D tilemapCollider;

    PlayerController player;

    public const float MAX_COLLISION_TIME = 0.7f;

    [SerializeField]
    float currentCollisionTime;
    float collisionStoppingSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = transform.GetComponent<Tilemap>();
        availablePlaces = new List<Vector3>();

        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);
                }
            }
        }

        //for (int i = 0; i < availablePlaces.Count; i++)
        //{
        //    Debug.Log(availablePlaces[i]);
        //}

        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    public void TilePassCheck()
    {
        if (tilemapCollider.enabled)
        {
            float curPositionX = Mathf.Round(player.transform.position.x);
            float curPositionY = Mathf.Round(player.transform.position.y) + 0.5f;

            for (int i = 0; i < availablePlaces.Count; i++)
            {
                if ((curPositionX == availablePlaces[i].x) &&
                    (Mathf.Abs(curPositionY) == Mathf.Abs(availablePlaces[i].y) - 0.5f))
                {
                    Debug.Log("Passing");
                    player.isAirColliderPassing = true;
                    tilemapCollider.enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemapCollider.enabled)
        {
            if (currentCollisionTime < MAX_COLLISION_TIME)
            {
                currentCollisionTime += collisionStoppingSpeed;
            }
            else
            {
                if (!tilemapCollider.enabled)
                {
                    tilemapCollider.enabled = true;
                    player.isAirColliderPassing = false;
                }
                currentCollisionTime = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TilePassCheck();
        }
    }
}
