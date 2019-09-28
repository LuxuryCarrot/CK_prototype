using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherMonster : MonoBehaviour
{
    //몬스터의 상태정보 있는 eNUMERABLE 변수 선언
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;

    //각종 컴포넌트 미리 저장
    private Transform monsterTr;
    private Transform playerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;


    //추적 거리.
    public float traceDist = 10f;
    //공격 거리.
    public float attackDist = 2f;
    //몬스터 사망 여부
    private bool isDie = false;

    // Use this for initialization
    void Start()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //일정한 간격으로 몬스터의 행동상태를 체크
        StartCoroutine(this.CheckMonsterState());
        //몬스터의 상태에 따른 동작
        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            //몬스터와 플레이어 거리측정.
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)   //공격 범위 이내에 들어왔는가?
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist) //추적 범위 이내에 들어왔는가?
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    //추적중지.
                    nvAgent.Stop();
                    break;
                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    break;
                case MonsterState.attack:
                    break;
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
