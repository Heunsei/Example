using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TextMesh Pro 관련 컴포넌트에 접근
using TMPro;

public class GameManager : MonoBehaviour
{
    //monster가 출현할 위치를 저장할 List타입 변수
    public List<Transform> points = new List<Transform>();

    //몬스터를 미리 생성해 저장할 리스트 자료형
    public List<GameObject> monsterPool = new List<GameObject> ();

    //오브젝트 풀 에 생성할 몬스터의 최대 개수
    public int maxMonsters = 10;

    //몬스터 프리팹을 연결할 변수
    public GameObject monster;

    //몬스터의 생성 간격
    public float createTime = 3.0f;

    //게임의 종료 여부를 저잘 멤버 함수
    public bool isGameOver;

    //게임의 종료 여부를 저장할 프로퍼티
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {   
                //실행한 함수 종료.
                CancelInvoke("CreateMonster");
            }
        }
    }

    //점수기록을 위한 텍스트 연결 변수
    public TMP_Text scoreText;
    //최고 점수 기록
    private int totScore = 0;

    //싱글턴 인스턴스 선언
    //static 으로 선언하여 메모리에 상주
    //외부접근이 가능하도록 public
    public static GameManager instance = null;

    //스크립트가 실행되면 가장먼저 호출되는 유니티 이벤트 함수
    void Awake()
    {   

        if(instance == null)
        {
            instance = this;
        }
        //instance에 할당된 클래스 인스턴스가 다를 경우 새롭게 생성된 클래스를 의미
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        //다른 씬으로 넘어가더라도 삭제하지 않고 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        // SpawnPointGroup 게임오브젝트의 Transform 컴포넌트 추출
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //SpawnPointGroup 하위에 있는 모든 차일드 게임오브젝트의 Transform 컴포넌트 추출
        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);
        foreach(Transform point in spawnPointGroup)
        {
            points.Add(point);
        }


        //일정한 시간 간격으로 함수를 호출
        //Invoke("호출할 함수", 대기_시간);
        //InvokeRepeating("호출할 함수", 대기_시간, 호출_간격);

        InvokeRepeating("CreateMonster", 2.0f, createTime);

        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DisplayScore(0);
  
    }
    //몬스터 생성
        void CreateMonster()
        {
            //몬스터의 불규칙한 생성 위치 산출
            int idx = Random.Range(0,points.Count);
            //몬스터 프리팹 생성
            //Instantiate(monster, points[idx].position, points[idx].rotation );
            //오브젝트 풀에서 몬스터 추출
            GameObject _monster = GetMonsterInPool();
            //추출한 몬스터의 위치와 회전을 설정
            _monster?.transform.SetPositionAndRotation(points[idx].position,
                                                   points[idx].rotation);
            // 추출한 몬스터를 활성화
            _monster.SetActive(true);
            
        }

    void CreateMonsterPool()
    {
        for(int i =0; i < maxMonsters; i++)
        {
            //몬스터 생성
            var _monster = Instantiate<GameObject>(monster);
            //몬스터의 이름 저장
            _monster.name = $"Monster_i{i:00}";
            //몬스터 비활성화
            _monster.SetActive(false);

            //생성한 몬스터를 오브젝트 풀에 추가
            monsterPool.Add(_monster);
        }
    }

    //object pool에서 사용 가능한 몬스터를 추출해 반환하는 함수
    public GameObject GetMonsterInPool()
    {
        //오브젝트 풀의 처음부터 끝까지 순회
        foreach(var _monster in monsterPool)
        {
            //비활성화 여부로 사용 가능한 몬스터를 판단
            if(_monster.activeSelf == false)
            {
                //몬스터 반환
                return _monster;
            }
        }
        return null;
    }

    //정보 누적 및 표기
    // PlayerPrefs는 보안성 0;
    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totScore:#,##0} </color>";
        //score save
        PlayerPrefs.SetInt("TOT_SCORE",totScore);
    }

}
