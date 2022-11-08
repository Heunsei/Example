using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();     
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        //회전 실행
        Rotate();
        //움직임 실행
        Move();

        //입력값에 따라 애니메이터 Move파라미터값 변경
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        Vector3 moveDistance =
            playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //rigidbody 사용으로 게임 오브젝트 위치 변경 Moveposition > 전역위치사용
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        //transform.position = transform.position + moveDistance; 
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {   
        //한 프레임동안 회전한 각도 저장 (입력 , 회전속도 , 시간)
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        //rigidbody 사용으로 게임 오브젝트 회전 변경
        playerRigidbody.rotation =     //회전값의 덧셈은 곱으로 표현
            playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);

    }
}