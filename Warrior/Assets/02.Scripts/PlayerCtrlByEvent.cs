using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrlByEvent : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction attackAcion;

    private Animator anim;
    private Vector3 moveDir;

    void Start()
    {
        anim = GetComponent<Animator>();

        //Move 액션 생성 및 타입 설정
        moveAction = new InputAction("Move", InputActionType.Value);

        //move 액션의 복합 바인딩 정보 정의
        moveAction.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        //move 액션의 performed,canceled 이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //warrior run 애니메이션 실행
            anim.SetFloat("Movement", dir.magnitude);
        };

        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };

        moveAction.Enable();

        attackAcion = new InputAction("Attack",
                                       InputActionType.Button,
                                       "<Keyboard>/space");
        //attack액션의 performed 이벤트 연결
        attackAcion.performed += ctx =>
        {
            anim.SetTrigger("Attack");
        };
        attackAcion.Enable();
    }

    void Update()
    {
        if(moveDir != Vector3.zero)
        {
            //진행방향으로 전환
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전한 후 전진 방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }
}
