using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{   //따라가야할 대상을 연결 할 변수
    public Transform targetTr;
    //maincamera 자신의 transform 컴포넌트
    private Transform camTr;

    //Range > 유니티 인스펙터뷰에 최대값 최소값 설정
    [Range(0.0f, 10.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 2.0f)]
    public float height = 2.0f;
    //반응속도
    public float dumping = 10.0f;

    private Vector3 velocity = Vector3.zero;

    public float targetOffset = 2.0f;


    void Start()
    {   
        camTr = GetComponent<Transform>();
    }


    void LateUpdate()
    {
        Vector3 pos = targetTr.position     
                    + (-targetTr.forward * distance)
                    + (Vector3.up * height);

        //카메라 위치 조정, 구면선형보간 사용
        //camTr.position = Vector3.Slerp(camTr.position,              //시작위치
        //                               pos,                         //목표지점
        //                               Time.deltaTime * dumping);   //시간 t
        
        //smoothdamp를 통한 위치보간.
        camTr.position = Vector3.SmoothDamp(camTr.position,     //시작위치
                                            pos,                //목표지점
                                            ref velocity,       //현재속도
                                            dumping);           //도달까지 걸릴시간

        //targetTr을향해서 각도조절
        camTr.LookAt(targetTr.position + targetTr.up * targetOffset);
    }
}
