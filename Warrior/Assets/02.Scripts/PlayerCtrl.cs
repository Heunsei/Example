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

        //ActionMap����
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");

        //move, attack �׼� ����
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");

        //Move �׼��� performed �̺�Ʈ ����
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //warrior_Run����
            anim.SetFloat("Movement", dir.magnitude);
        };
        //Move �׼��� cancled �̺�Ʈ ����
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            //warrior_run �ִϸ��̼� ����
            anim.SetFloat("Movement", 0.0f);
        };

        //Attack �׼��� performed �̺�Ʈ ����
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
            //����������� ȸ��
            transform.rotation = Quaternion.LookRotation(moveDir);
            //ȸ�� �� ������������ �̵�
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    //#region
    //�ڵ��� ������ ���� �ϴ°�, Ư�� ������ collapse����
    //#endregion
    #region SEND_MESSAGE
    //inputValue > Get() or Get<t>() �������� ���� ����
    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        //2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
        moveDir = new Vector3(dir.x, 0, dir.y);

        //animation ����
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
    //CallbackContext.phase�� ����� �׼��� ȣ�� ���¸� �� �� ����.
    //InputActionPhase.started, Performed, Canceled, Disabled, Waiting.
    public void OnMove(InputAction.CallbackContext ctx)
    {   
        //invoke unity event �ɼ� ���� �� �Ѿ���� �Ķ���Ͱ� InputAction.CallbackContext Ÿ��
        //�Է°��� ReadValue<T>()�Լ��� ���߹���
        Vector2 dir = ctx.ReadValue<Vector2>();

        //2���� ��ǥ�� 3�������� ��ȯ
        moveDir = new Vector3(dir.x, 0, dir.y);
        //animation ����
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
