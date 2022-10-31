using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{   
    //폭발 효과를 연동할 변수
    public GameObject expEffect;
    //컴포넌트를 저장할 변수 선언
    private Transform tr;
    private Rigidbody rb;
    public Texture[] textures;
    //폭발 반경
    public float radius = 10.0f;

    private new MeshRenderer renderer;

    //총알 누적 변수
    private int hitCount = 0;

    void Start()
    {
       tr = GetComponent<Transform>();
       rb = GetComponent<Rigidbody>(); 
       //하위 meshcomponent 추출
       renderer = GetComponentInChildren<MeshRenderer>();

       //난수발생
       int idx = Random.Range(0, textures.Length);
       //텍스트 지정
       renderer.material.mainTexture = textures[idx]; 

    }


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            //총알 맞은 횟수를 증가시키고 3이상이면 폭발
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        //폭발 효과 파티클 생성
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        //폭발 효과 파티클 제거
        Destroy(exp, 3.0f);

        //RigidBody 컴포넌트의 mass를 1.0으로 수정해 무게를 가볍게 함
        //rb.mass = 1.0f;
        //위로 솟구치는 힘 입력
        //rb.AddForce(Vector3.up * 1500.0f);

        //간접 폭발력 전달.
        IndirectDamage(tr.position);

        //3초 후 드럼통 제거
        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        //주변의 드럼통 추출 
        //overlapsphere (원점,반지름,검출 대상 레이어)
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach(var coll in colls)
        {
            //폭발 범위 내의 rigidbody 컴포넌트 추출
            rb = coll.GetComponent<Rigidbody>();
            //드럼통 무게 변경
            rb.mass = 1.0f;
            //freezRotation 제한값 해제
            rb.constraints = RigidbodyConstraints.None;
            //폭발력 전달
            //AddExploisingForce(횡 폭발력, 폭발원점, 폭발반경, 종 폭발력)
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
