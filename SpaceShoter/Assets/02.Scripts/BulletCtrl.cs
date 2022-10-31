using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 10.0f;

    public float force = 1500.0f;

    private Rigidbody rb;

    void Start()
    {   //rigidbody 컴포넌트를 추출
        rb = GetComponent<Rigidbody>();
        
        //전진방향으로 force만큼 힘을 가한다
        rb.AddForce(transform.forward * force);

        //force함수 
        //void AddForce(Vector3 force);
        //void AddRelativeForce(Vector3 force);
    }

    
    void Update()
    {
        
    }
}
