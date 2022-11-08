using UnityEngine;

//Scriptable Object
//여러 오브젝트가 사용할 데이터를 유니티 에셋 형태로 저장

//여러 오브젝트가 공유하여 사용할 데이터를 에셋 형태로 분리
//데이터를 유니티 인스펙터 창에서 편집 가능한 형태로 관리

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")]
//[CreateAssetMenu(menuName = "메뉴경로",filename = "기본파일명",order = 메뉴상 순서)]
public class GunData : ScriptableObject

// GunData 클래스가 ScriptableObject클래스를 상속시키도록 설정

{
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력

    public int startAmmoRemain = 100; // 처음에 주어질 전체 탄약
    public int magCapacity = 25; // 탄창 용량

    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
}