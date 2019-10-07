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

    public int index;

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
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(tileMap.GetCellCenterWorld(localPlace));
                }
            }
        }

        //for(int i=0; i<availablePlaces.Count; i++)
        //{
        //    Debug.Log(availablePlaces[i]);
        //}

        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    public void TilePassCheck()
    {
        if (!tilemapCollider.enabled && !player.isAirColliderPassing)
        {
            float curRoundPositionX = Mathf.Round(player.transform.position.x);
            float curRoundPositionY = Mathf.Round(player.transform.position.y);      //-

            //Debug.Log("curPositionX , curPositionY : " + curPositionY + ", " + curPositionY);

            for (int i = 0; i < availablePlaces.Count; i++)
            {
                float tempX = Mathf.Abs(curRoundPositionX) - Mathf.Abs(availablePlaces[i].x);
                float tempY = Mathf.Abs(curRoundPositionY) - Mathf.Abs(availablePlaces[i].y);

                if ((Mathf.Abs(tempX) == 0.5f) &&
                    (Mathf.Abs(tempY) == 0.5f))       //+
                {
                    Debug.Log("PassingStart");
                    index = i;
                    player.isAirColliderPassing = true;
                    StartCoroutine("TileMapColliderEnable");
                    break;
                }
            }
        }
    }

    public void TilePassEndCheck()      //passing 이 되면 실행한다
    {
        if (player.isAirColliderPassing)
        {
            float roundCurPositionX = Mathf.Round(player.transform.position.x * 10.0f) / 10.0f;
            float roundCurPositionY = Mathf.Round(player.transform.position.y * 10.0f) / 10.0f;
            //float roundCurPositionX = Mathf.Round(player.transform.position.x);
            //float roundCurPositionY = Mathf.Round(player.transform.position.y);


            float tempX = Mathf.Abs(roundCurPositionX) - Mathf.Abs(availablePlaces[index].x);
            float tempY = Mathf.Abs(roundCurPositionY) - Mathf.Abs(availablePlaces[index].y);

            if ((Mathf.Abs(tempX) != 0f) ||
                  (Mathf.Abs(tempY) != 0f))
            {
                Debug.Log("PassingEnd");
                player.isAirColliderPassing = false;
                player.isAirColliderPassingEnd = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isAirColliderPassingEnd && !tilemapCollider.enabled && !player.isAirColliderPassing)
        {
            tilemapCollider.enabled = true;
            player.isAirColliderPassingEnd = false;
        }

        TilePassCheck();
        TilePassEndCheck();
    }


    IEnumerator TileMapColliderEnable()
    {
        yield return new WaitForSeconds(1.0f);
        if (!tilemapCollider.enabled&& !player.isAirColliderPassingEnd)
        {
            tilemapCollider.enabled = true;
        }
    }
}
