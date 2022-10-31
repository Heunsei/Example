using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private Animation anim;

    private Transform tr;
    //이동속도 변수
    public float moveSpeed = 10.0f;
    //회전속도 변수
    public float turnSpeed = 10.0f;

    //초기 생명력 값
    private readonly float initHp = 100.0f;
    //현재 생명력 값
    public float currHp;
    //hp bar를 연결할 변수
    private Image hpBar;
    

    //델리게이트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        //hpbar 연결
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        //hp 초기화
        currHp = initHp;
        DisplayHealth();
        anim = GetComponent<Animation>();
        tr = GetComponent<Transform>();

        //애니메이션 실행
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    void Update()
    {       
        float h = Input.GetAxis("Horizontal"); 
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        //전후좌우 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //Translate(이동방향,속력,Time.deltaTime)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        //Vector3.up 축을 기준으로 turnspeed 만큼 속도로 회전
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r * 5);
        //주인공 캐릭터의 애니메이션 설정
        PlayerAnim(h, v);

    }

    void PlayerAnim(float h,float v)
    {
        //키보드 입력값을 기준으로 동작할 애니메이션 수행
        if(v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if(currHp >= 0.0f && coll.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            Debug.Log($"Player hp = {currHp / initHp}");
            DisplayHealth();
            // Debug.Log($"Player Hp : {currHp}/{initHp}={currHp/initHp}");
            Debug.Log($"Player hp = {currHp / initHp}");
            //체력이 0이하가 되면 사망처리
            if(currHp < 0.0f)
            {
                playerDie();
            }
        }
    }
    void playerDie()
    {
        Debug.Log("Player Die ! ");

        //monster 태그를가진 모든 게임 오브젝트를 찾아옴
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //foreach(GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        OnPlayerDie();

        // GameManage 스크립트의 IsGameOver 프로퍼티 값을 변경
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;

    }
    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}
