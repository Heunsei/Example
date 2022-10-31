using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;
    private void OnCollisionEnter(Collision coll)
    {   //충돌한 게임 오브젝트와 비교 
        if(coll.collider.CompareTag("BULLET"))
        {   
            //첫번째 충돌 지점의 정보 추출
            ContactPoint cp = coll.GetContact(0);
            //충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);
            //스파크 파티클을 동적으로 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //스파크를 제거
            Destroy(spark, 0.5f);
            //게임 오브젝트 제거
            Destroy(coll.gameObject);   
        }
    }
}
