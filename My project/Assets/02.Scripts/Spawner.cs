using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3.0f;

    private Transform target; //�߻� ���
    private float spawnRate; //���� �ֱ�
    private float timeAfterSpawn; //�ֱ� ���� �������� ���� �ð�
    void Start()
    {   
        //�������� �����ð� 0���� �ʱ�ȭ
        timeAfterSpawn = 0.0f;
        //ź�� �������� ����
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        //Ÿ�� ����
        target = FindObjectOfType<PlayerCtrl>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;
        if(timeAfterSpawn > spawnRate)
        {
            timeAfterSpawn = 0;
            //bulletprefabs�� ���纻�� �ش� ��ġ���� ����
            GameObject bullet = Instantiate(bulletPrefab,transform.position,transform.rotation);

            bullet.transform.LookAt(target);

            //������ ���� ������ spawnRateMin ~ spawnRateMax���̿��� ��������
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
       
    }
}
