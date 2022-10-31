using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float bulletSpeed = 5.0f;
    private Rigidbody bulletRigidbody;
  
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * bulletSpeed;

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //���� ���� ������Ʈ���� PlayerCtrl ������Ʈ�� �����ؿ�
            PlayerCtrl playerctrl = other.GetComponent<PlayerCtrl>();
            if(playerctrl != null)
            {
                playerctrl.Die();
            }
        }
    }
}
