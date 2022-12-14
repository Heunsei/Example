#pragma warning disable IDE0051

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private Animator anim;
    private new Transform transform;
    private Vector3 moveDir;

    private PlayerInput playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;


    void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();

        //ActionMap추출
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");

        //move, attack 액션 추출
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");

        //Move 액션의 performed 이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //warrior_Run실행
            anim.SetFloat("Movement", dir.magnitude);
        };
        //Move 액션의 cancled 이벤트 연결
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            //warrior_run 애니메이션 정지
            anim.SetFloat("Movement", 0.0f);
        };

        //Attack 액션의 performed 이벤트 연결
        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };

    }

    void Update()
    {
        if(moveDir != Vector3.zero)
        {
            //진행방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전 후 전진방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    //#region
    //코드의 영역을 정의 하는것, 특정 영역을 collapse가능
    //#endregion
    #region SEND_MESSAGE
    //inputValue > Get() or Get<t>() 형식으로 값을 전달
    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        //2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);

        //animation 실행
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }
    
    void OnAttack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
    #endregion

    #region UNITY_EVENTS
    //CallbackContext.phase를 사용해 액션의 호출 상태를 알 수 있음.
    //InputActionPhase.started, Performed, Canceled, Disabled, Waiting.
    public void OnMove(InputAction.CallbackContext ctx)
    {   
        //invoke unity event 옵션 설정 시 넘어오는 파라미터가 InputAction.CallbackContext 타입
        //입력값을 ReadValue<T>()함수로 전발받음
        Vector2 dir = ctx.ReadValue<Vector2>();

        //2차원 좌표를 3차원으로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);
        //animation 실행
        anim.SetFloat("Movement", dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase={ctx.phase}");
        //
        if (ctx.performed)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }
    #endregion
}
