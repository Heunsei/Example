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

        //Move �׼� ���� �� Ÿ�� ����
        moveAction = new InputAction("Move", InputActionType.Value);

        //move �׼��� ���� ���ε� ���� ����
        moveAction.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        //move �׼��� performed,canceled �̺�Ʈ ����
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //warrior run �ִϸ��̼� ����
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
        //attack�׼��� performed �̺�Ʈ ����
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
            //����������� ��ȯ
            transform.rotation = Quaternion.LookRotation(moveDir);
            //ȸ���� �� ���� �������� �̵�
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }
}
