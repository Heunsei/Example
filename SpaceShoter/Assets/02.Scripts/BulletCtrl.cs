using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 10.0f;

    public float force = 1500.0f;

    private Rigidbody rb;

    void Start()
    {   //rigidbody ������Ʈ�� ����
        rb = GetComponent<Rigidbody>();
        
        //������������ force��ŭ ���� ���Ѵ�
        rb.AddForce(transform.forward * force);

        //force�Լ� 
        //void AddForce(Vector3 force);
        //void AddRelativeForce(Vector3 force);
    }

    
    void Update()
    {
        
    }
}
