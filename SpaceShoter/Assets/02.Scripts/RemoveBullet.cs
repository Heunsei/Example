using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;
    private void OnCollisionEnter(Collision coll)
    {   //�浹�� ���� ������Ʈ�� �� 
        if(coll.collider.CompareTag("BULLET"))
        {   
            //ù��° �浹 ������ ���� ����
            ContactPoint cp = coll.GetContact(0);
            //�浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);
            //����ũ ��ƼŬ�� �������� ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //����ũ�� ����
            Destroy(spark, 0.5f);
            //���� ������Ʈ ����
            Destroy(coll.gameObject);   
        }
    }
}
