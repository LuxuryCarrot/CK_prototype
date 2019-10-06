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

    public const float MAX_COLLISION_TIME = 1f;

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
                    Debug.Log(tileMap.GetCellCenterWorld((new Vector3Int(n, p, (int)tileMap.transform.position.y))));
                }
            }
        }


        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    public void TilePassCheck()
    {
        if (tilemapCollider.enabled && !player.isAirColliderPassing)
        {
            float curPositionX = Mathf.Round(player.transform.position.x);
            float curPositionY = Mathf.Round(player.transform.position.y)/2;      //-

            for (int i = 0; i < availablePlaces.Count; i++)
            {
                if ((curPositionX == availablePlaces[i].x) &&
                    (Mathf.Abs(curPositionY) == Mathf.Abs(availablePlaces[i].y)/2))       //+
                {
                    Debug.Log("Passing");
                    tilemapCollider.enabled = false;
                    player.isAirColliderPassing = true;
                }
            }
        }
    }

    public void TilePassEndCheck()      //passing 이 되면 실행한다
    {
        if (player.isAirColliderPassing)
        {
            float curPositionX = Mathf.Round(player.transform.position.x);
            float curPositionY = Mathf.Round(player.transform.position.y)/2;

            for (int i = 0; i < availablePlaces.Count; i++)
            {
                if ((curPositionX != availablePlaces[i].x) &&
        (Mathf.Abs(curPositionY) != Mathf.Abs(availablePlaces[i].y)/2))
                {
                    if (i == availablePlaces.Count - 1)
                    {
                        Debug.Log("PassingEnd");
                        player.isAirColliderPassing = false;
                        player.isAirColliderPassingEnd = true;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 시간초로 컬라이더를 끄는 식으로 하면  끝날때 아직 플레이어가 박스안에 있을때 레이를 쏘고 박스에 걸려서 안에 갇힘

        /*
        플레이어가 타일 passing조건 값과 같다면 passing 시작하고
        passing을 시작하고나서 계속 비교한다
        그리고 passing 조건을 벗어났다면 플레이어가 타일을 벗어났다는 뜻이므로 그때 passing end
        passing end가 되면 tileMap 의  컬라이더는 켜지고 player의 바닥체크를 실행한다. 실행조건용으로  passingEnd라는 변수가 필요할듯

        passing end가 됐을때도 근접해서 레이로 air가 걸릴때는 ?
        passing end가 됐으면 시간계산?

        기존 시간으로 collider를 끄는 코드는?
        tileMap collider를 끄는 조건은 한가지다 passing조건이 만족했을때 
        그렇다면 passing 좌표를 벗어났다면 컬라이더를 끄게 만드는게 좋을듯

        passing 조건의 좌표를 더이상 바꾸긴 힘듬 그때 육안상으로 제대로 닿지 않았는데도 passing 이 실행될때가 있다
        1. onCollisionEnter의 가능성 -> 리소스의 낭비지만 업데이트에서 조건을 계속 검색하는 방법
        2. 그래도 안될시 플레이어의 충돌범위를 조금 줄일수 밖에 없음 (단점은 발이 땅에 떠있는게 보인다)

        현재 코드의 문제는 passing 조건은 만족했으나  업데이트에서 컬라이더가 꺼지자 마자 바로 시간계산을 진행했고 그사이에 collider의 passing에  false를 주고 컬라이더를 켜줬다?
        그렇다면 일단 타일의 컬라이더를 한번 이상은 껐다는 뜻으로 순서는  passing 먼저 진행됐다는 뜻 
        즉 패싱이 되자마자 시간계산하는게 문제 패싱하는 순간에 passing=true, 컬라이더를 끄는건 좋은 선택이 아니다 
        즉 요약하자면 패싱중에 하는게 아니라 passing start-> passing end가 되면 바로 끄든 시간으로 끄든 선택할것 
         */


        if (player.isAirColliderPassingEnd && !tilemapCollider.enabled && !player.isAirColliderPassing)
        {
             tilemapCollider.enabled = true;
            player.isAirColliderPassingEnd = false;
        }

        TilePassCheck();
        TilePassEndCheck();

        //if (!tilemapCollider.enabled)
        //{
        //    if (currentCollisionTime < MAX_COLLISION_TIME)
        //    {
        //        currentCollisionTime += collisionStoppingSpeed;
        //    }
        //    else
        //    {
        //        if (!tilemapCollider.enabled)
        //        {
        //            tilemapCollider.enabled = true;
        //            player.isAirColliderPassing = false;
        //        }
        //        currentCollisionTime = 0;
        //    }
        //}
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        TilePassCheck();
    //    }
    //}
}
