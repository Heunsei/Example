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

        //씬에 존재하지 않는 GameManager타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();
        //가져온 GameManager 오브젝트의 EndGame()실행
        gameManager.EndGame();
    }
}
