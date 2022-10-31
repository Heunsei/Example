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
            //상대방 게임 오브젝트에서 PlayerCtrl 컴포넌트를 추출해옴
            PlayerCtrl playerctrl = other.GetComponent<PlayerCtrl>();
            if(playerctrl != null)
            {
                playerctrl.Die();
            }
        }
    }
}
