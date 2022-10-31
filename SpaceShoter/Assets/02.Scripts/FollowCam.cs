using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{   //���󰡾��� ����� ���� �� ����
    public Transform targetTr;
    //maincamera �ڽ��� transform ������Ʈ
    private Transform camTr;

    //Range > ����Ƽ �ν����ͺ信 �ִ밪 �ּҰ� ����
    [Range(0.0f, 10.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 2.0f)]
    public float height = 2.0f;
    //�����ӵ�
    public float dumping = 10.0f;

    private Vector3 velocity = Vector3.zero;

    public float targetOffset = 2.0f;


    void Start()
    {   
        camTr = GetComponent<Transform>();
    }


    void LateUpdate()
    {
        Vector3 pos = targetTr.position     
                    + (-targetTr.forward * distance)
                    + (Vector3.up * height);

        //ī�޶� ��ġ ����, ���鼱������ ���
        //camTr.position = Vector3.Slerp(camTr.position,              //������ġ
        //                               pos,                         //��ǥ����
        //                               Time.deltaTime * dumping);   //�ð� t
        
        //smoothdamp�� ���� ��ġ����.
        camTr.position = Vector3.SmoothDamp(camTr.position,     //������ġ
                                            pos,                //��ǥ����
                                            ref velocity,       //����ӵ�
                                            dumping);           //���ޱ��� �ɸ��ð�

        //targetTr�����ؼ� ��������
        camTr.LookAt(targetTr.position + targetTr.up * targetOffset);
    }
}
