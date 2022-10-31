using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//반드시 필요한 컴포넌트를 명시해 해당 컴포넌트가 삭제되는것을 방지하는 어트리뷰트
[RequireComponent(typeof(AudioSource))]

public class FireCtrl : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    //발사좌표
    public Transform firePos;
    //총소리에 사용할 오디오 음원
    public AudioClip fireSfx;
    //audioSource컴포넌트를 저장할 변수
    private new AudioSource audio;
    //Muzzle Flash의 meshRenderer 컴포넌트
    private MeshRenderer muzzleFlash;

    //레이캐스트 결과값 저장
    private RaycastHit hit;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //FirePos하위에 있는 meshRenderer 컴포넌트 추출
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        //처음 시작시 비활성화
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            if (Physics.Raycast(firePos.position,   //광선의 발사 원점
                                firePos.forward,    //방향
                                out hit,            //결과 데이터 
                                10.0f,              //광선의 거리
                                1 << 6))           //감지하는 범위인 레이어 마스크
            { 
                Debug.Log($"HIT={hit.transform.name}");
                hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);
            }
        }
    }

    void Fire()
    {   
        //프리팹을 동적으로 생성 (생성할 객체, 위치, 회전)
        Instantiate(bullet, firePos.position, firePos.rotation);
        //총소리 발생
        //audiosource.playoneshot(오디오클립, 볼륨)
        audio.PlayOneShot(fireSfx, 1.0f);
        //총구 화염 효과 코루틴 함수 호출
        StartCoroutine(showMuzzleFlash());
    }

    IEnumerator showMuzzleFlash()
    {
        //오프셋 좌표값을 랜덤 함수로 설정
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //텍스쳐의 오프셋 값 설정
        muzzleFlash.material.mainTextureOffset = offset;

        //muzzleFlash의 회전 변경
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0,0,angle);

        //muzzleFlash의 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale; 

        //muzzleFlash 활성화
        muzzleFlash.enabled = true;
        //0.2초 동안 대기하는 동안 메세지 루프로 제어권을 양보
        yield return new WaitForSeconds(0.2f);
        //muzzleFlash 비활성화
        muzzleFlash.enabled = false;
    }
}
