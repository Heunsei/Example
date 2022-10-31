using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내비게이션 기능을 사용하기 위해 추가하는 네임스페이스
using UnityEngine.AI;


public class MonsterCtrl : MonoBehaviour
{   
    // component 캐시 처리
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    //혈흔효과 프리팹
    private GameObject bloodEffect;
    //몬스터의 체력
    private int hp = 100;


    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    // 몬스터의 현재 상태
    public State state = State.IDLE;

    // 추적 사정거리
    public float traceDist = 10.0f;

    // 공격 사정거리
    public float attackDist = 2.0f;

    // 몬스터의 사망 여부
    public bool isDie = false;

    void OnEnable()
    {   
        //PlayerCtrl의 OnPlayerDie와 MonsterCtrl의 OnPlayerDie연결
        //이벤트 연결, 이벤트가 선언된 클래스명.이벤트명 += 발생시 호출할 함수
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        
        //추적 대상의 위치를 설정하면 바로 추격 시작
        //agent.destination = playerTr.position;

        //몬스터의 상태를 체크하는 코루틴함수 
        StartCoroutine(CheckMonsterState());
        //상태에 따라 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(MonsterAction());
    }

    void OnDisable()
    {
        //이벤트 해지, 이벤트가 선언된 클래스명.이벤트명 -= 발생시 호출할 함수
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    void Awake()
    {
        //monster의 transform할당
        monsterTr = GetComponent<Transform>();

        //player의 transform할당
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        //NavMeshAgent component 할당
        agent = GetComponent<NavMeshAgent>();
        //navmeshagent의 자동회전 기능 비활성화
        agent.updateRotation = false;

        //Animatior component 할당
        anim = GetComponent<Animator>();

        //blood 프리팹 로드
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    void Update()
    {
        //목적지까지 남은 거리로 회전 여부 판단
        if(agent.remainingDistance >= 2.0f)
        {
            //에이전트의 이동 방향
            Vector3 direction = agent.desiredVelocity;
            //회전각 산출
            Quaternion rot = Quaternion.LookRotation(direction);
            //구면 선형보간 함수로 부드러운 회전처리
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation,
                                                    rot,
                                                    Time.deltaTime * 10.0f);
        }
    }

 

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 0.3동안 중지(대기) 하는 동안 제어권을 메시지 루프에 양보
            yield return new WaitForSeconds(0.3f);
            //몬스터의 상태 체크
            if(state == State.DIE) yield break;
            // 몬스터와 주인공 캐릭터 사이의 거리 측정
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            // 공격 사정거리 범위로 들어왔는지 확인.
            if(distance <= attackDist)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 범위로 들어왔는지 확인.
            else if(distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE상태
                case State.IDLE:
                    // 추적중지
                    agent.isStopped = true;
                    // animator의 istrace 변수를 false로 설정
                    anim.SetBool(hashTrace, false);
                    break;

                //추적상태
                case State.TRACE:
                    // 추적 대상의 좌표로 이동 시작.
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                    // animator의 istrace 변수를 true로 설정
                    anim.SetBool(hashTrace, true);

                    // animator의 isAttack 변수를 false로 설정
                    anim.SetBool(hashAttack, false);
                    break;

                // 공격상태
                case State.ATTACK:
                    // animator의 isAttack 변수를 true로 설정
                    anim.SetBool(hashAttack, true);
                    break;

                // 사망
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    // collider 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;

                    //일정시간 대기후 오브젝트 풀링으로 환원
                    yield return new WaitForSeconds(3.0f);

                    //사망 후 다시 사용할 떄를 위해 hp값 초기화
                    hp = 100;
                    isDie = false;

                    //몬스터의 Collider 활성화
                    GetComponent<CapsuleCollider>().enabled = true;
                    //몬스터를 비활성화
                    this.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            //충돌 총알 삭제
            Destroy(collision.gameObject);
            //피격 애니메이션 실행
            anim.SetTrigger(hashHit);
            

            //총알의 충돌 지점
            Vector3 pos = collision.GetContact(0).point;
            //총알의 충돌 지점의 법선 벡터
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            //혈흔 효과를 생성하는 함수 호출'
            ShowBloodEffect(pos, rot);

            hp -= 10;
            if (hp <= 0)
            {
                state = State.DIE;
                GameManager.instance.DisplayScore(50);
            }
        }
    }
   void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.gameObject.name);
    }

    void ShowBloodEffect(Vector3 pos , Quaternion rot)
    {
        //Instantiate(생성객체, 위치 , 각도, 부모게임 오브젝트)
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        //1초뒤 blood gameObejcet 파괴
        Destroy(blood, 1.0f);
    }


    void OnDrawGizmos()
    {
        //추적 사정거리 표시
        if(state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        //공격 사정거리 표시
        if(state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }    

    //몬스터의 상태를 변경하고 행동하는 코루틴함수 정지 및 NavMeshAgent component도 정지
    void OnPlayerDie()
    {
        //몬스터의 상태를 체크하는 코루틴 함수를 전부 정지
        StopAllCoroutines();

        //추적을 정지하고 애니메이션을 수행
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }

    //raycast를 사용해 데미지를  입히는 logic
    public void OnDamage(Vector3 pos,Vector3 normal)
    {   
        //피격 애니메이션 재생
        anim.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);

        ShowBloodEffect(pos, rot);

        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;
            //몬스터 사망시 50점 추가
            GameManager.instance.DisplayScore(50);
        }
    }
}
