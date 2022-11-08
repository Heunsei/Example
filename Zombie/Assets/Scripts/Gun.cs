using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태 / 외부에서 state의 값 임의로 변경 불가능

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점, 연사 구현시 사용
  
    private void Awake()
    {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 총이 활성화 될 때 상태 초기화
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;

        //총의 현재 상태를 사용 준비된 상태로 변경
        state = State.Ready;
        //마지막으로 총을 쏜 시점 초기화
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire()
    {
        //현재 상태가 발사 가능한 상태
        //마지막 발사시점인 lastFireTime 이상의 시간이 지남
        if(state ==State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            //발사처리 실행
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {   
        //RayCast저장 변수
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        //레이캐스트(시작지점,방향,충돌정보 컨테이너,사정거리)
        //out키워드로 입력된 변수는 메소드 내부에서 변경된 사항이 반영된 채 되돌아옴
        if(Physics.Raycast(fireTransform.position,fireTransform.forward,out hit, fireDistance))
        {
            //레이어와 충돌시
            //충돌 오브젝트에서 IDamageable 오브젝트 가져오기
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                //OnDamage함수 실행
                target.OnDamage(gunData.damage, hit.point, hit.normal);
                //point > 충돌 위치, normal > 충돌 표면 방향 제공
            }
            //충돌 위치 저장
            hitPosition = hit.point;
        }
        else
        {   
            //충돌 x > 탈알이 끝까지 날아갔을 때 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        //탄알 수 감소
        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {   
        //총구 화염효과, 탄피배출 효과 재생
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        //PlayOneShot > 소리가 중첩되서 재생되더라도 앞의 오디오가 끊기지 않음.
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        //선의 시작점
        bulletLineRenderer.SetPosition(0, fireTransform.position);

        //선의 끝점
        bulletLineRenderer.SetPosition(1,hitPosition);

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        if(state == State.Reloading || ammoRemain <=0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
     
        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;

        if(ammoRemain <= ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        //재장전
        magAmmo += ammoToFill;

        //보유 총알 개수 감소
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}