using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{   
    //���� ȿ���� ������ ����
    public GameObject expEffect;
    //������Ʈ�� ������ ���� ����
    private Transform tr;
    private Rigidbody rb;
    public Texture[] textures;
    //���� �ݰ�
    public float radius = 10.0f;

    private new MeshRenderer renderer;

    //�Ѿ� ���� ����
    private int hitCount = 0;

    void Start()
    {
       tr = GetComponent<Transform>();
       rb = GetComponent<Rigidbody>(); 
       //���� meshcomponent ����
       renderer = GetComponentInChildren<MeshRenderer>();

       //�����߻�
       int idx = Random.Range(0, textures.Length);
       //�ؽ�Ʈ ����
       renderer.material.mainTexture = textures[idx]; 

    }


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            //�Ѿ� ���� Ƚ���� ������Ű�� 3�̻��̸� ����
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        //���� ȿ�� ��ƼŬ ����
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        //���� ȿ�� ��ƼŬ ����
        Destroy(exp, 3.0f);

        //RigidBody ������Ʈ�� mass�� 1.0���� ������ ���Ը� ������ ��
        //rb.mass = 1.0f;
        //���� �ڱ�ġ�� �� �Է�
        //rb.AddForce(Vector3.up * 1500.0f);

        //���� ���߷� ����.
        IndirectDamage(tr.position);

        //3�� �� �巳�� ����
        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        //�ֺ��� �巳�� ���� 
        //overlapsphere (����,������,���� ��� ���̾�)
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach(var coll in colls)
        {
            //���� ���� ���� rigidbody ������Ʈ ����
            rb = coll.GetComponent<Rigidbody>();
            //�巳�� ���� ����
            rb.mass = 1.0f;
            //freezRotation ���Ѱ� ����
            rb.constraints = RigidbodyConstraints.None;
            //���߷� ����
            //AddExploisingForce(Ⱦ ���߷�, ���߿���, ���߹ݰ�, �� ���߷�)
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
