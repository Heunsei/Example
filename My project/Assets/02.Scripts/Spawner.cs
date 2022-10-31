using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3.0f;

    private Transform target; //발사 대상
    private float spawnRate; //생성 주기
    private float timeAfterSpawn; //최근 생성 시점에서 지난 시간
    void Start()
    {   
        //생성이후 누적시간 0으로 초기화
        timeAfterSpawn = 0.0f;
        //탄알 생성간격 설정
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        //타겟 설정
        target = FindObjectOfType<PlayerCtrl>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;
        if(timeAfterSpawn > spawnRate)
        {
            timeAfterSpawn = 0;
            //bulletprefabs의 복사본을 해당 위치에서 생성
            GameObject bullet = Instantiate(bulletPrefab,transform.position,transform.rotation);

            bullet.transform.LookAt(target);

            //다음번 생성 간격을 spawnRateMin ~ spawnRateMax사이에서 랜덤지정
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
       
    }
}
