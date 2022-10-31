using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ݵ�� �ʿ��� ������Ʈ�� ����� �ش� ������Ʈ�� �����Ǵ°��� �����ϴ� ��Ʈ����Ʈ
[RequireComponent(typeof(AudioSource))]

public class FireCtrl : MonoBehaviour
{
    //�Ѿ� ������
    public GameObject bullet;
    //�߻���ǥ
    public Transform firePos;
    //�ѼҸ��� ����� ����� ����
    public AudioClip fireSfx;
    //audioSource������Ʈ�� ������ ����
    private new AudioSource audio;
    //Muzzle Flash�� meshRenderer ������Ʈ
    private MeshRenderer muzzleFlash;

    //����ĳ��Ʈ ����� ����
    private RaycastHit hit;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //FirePos������ �ִ� meshRenderer ������Ʈ ����
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        //ó�� ���۽� ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            if (Physics.Raycast(firePos.position,   //������ �߻� ����
                                firePos.forward,    //����
                                out hit,            //��� ������ 
                                10.0f,              //������ �Ÿ�
                                1 << 6))           //�����ϴ� ������ ���̾� ����ũ
            { 
                Debug.Log($"HIT={hit.transform.name}");
                hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);
            }
        }
    }

    void Fire()
    {   
        //�������� �������� ���� (������ ��ü, ��ġ, ȸ��)
        Instantiate(bullet, firePos.position, firePos.rotation);
        //�ѼҸ� �߻�
        //audiosource.playoneshot(�����Ŭ��, ����)
        audio.PlayOneShot(fireSfx, 1.0f);
        //�ѱ� ȭ�� ȿ�� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(showMuzzleFlash());
    }

    IEnumerator showMuzzleFlash()
    {
        //������ ��ǥ���� ���� �Լ��� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //�ؽ����� ������ �� ����
        muzzleFlash.material.mainTextureOffset = offset;

        //muzzleFlash�� ȸ�� ����
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0,0,angle);

        //muzzleFlash�� ũ�� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale; 

        //muzzleFlash Ȱ��ȭ
        muzzleFlash.enabled = true;
        //0.2�� ���� ����ϴ� ���� �޼��� ������ ������� �纸
        yield return new WaitForSeconds(0.2f);
        //muzzleFlash ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }
}
