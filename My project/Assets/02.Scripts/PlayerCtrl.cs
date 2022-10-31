using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Transform tr;
    public float speed = 8f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float hSpeed = h * speed;
        float vSpeed = v * speed;

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        //tr.Translate(moveDir.normalized * speed * Time.deltaTime);

        Vector3 newVelocity = new Vector3(hSpeed, 0, vSpeed);
        playerRigidbody.velocity = newVelocity;
    }
    
    public void Die()
    {
        gameObject.SetActive(false);

        //���� �������� �ʴ� GameManagerŸ���� ������Ʈ�� ã�Ƽ� ��������
        GameManager gameManager = FindObjectOfType<GameManager>();
        //������ GameManager ������Ʈ�� EndGame()����
        gameManager.EndGame();
    }
}
