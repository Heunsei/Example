using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private Animation anim;

    private Transform tr;
    //�̵��ӵ� ����
    public float moveSpeed = 10.0f;
    //ȸ���ӵ� ����
    public float turnSpeed = 10.0f;

    //�ʱ� ����� ��
    private readonly float initHp = 100.0f;
    //���� ����� ��
    public float currHp;
    //hp bar�� ������ ����
    private Image hpBar;
    

    //��������Ʈ ����
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        //hpbar ����
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        //hp �ʱ�ȭ
        currHp = initHp;
        DisplayHealth();
        anim = GetComponent<Animation>();
        tr = GetComponent<Transform>();

        //�ִϸ��̼� ����
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    void Update()
    {       
        float h = Input.GetAxis("Horizontal"); 
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        //�����¿� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //Translate(�̵�����,�ӷ�,Time.deltaTime)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        //Vector3.up ���� �������� turnspeed ��ŭ �ӵ��� ȸ��
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r * 5);
        //���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);

    }

    void PlayerAnim(float h,float v)
    {
        //Ű���� �Է°��� �������� ������ �ִϸ��̼� ����
        if(v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if(currHp >= 0.0f && coll.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            Debug.Log($"Player hp = {currHp / initHp}");
            DisplayHealth();
            // Debug.Log($"Player Hp : {currHp}/{initHp}={currHp/initHp}");
            Debug.Log($"Player hp = {currHp / initHp}");
            //ü���� 0���ϰ� �Ǹ� ���ó��
            if(currHp < 0.0f)
            {
                playerDie();
            }
        }
    }
    void playerDie()
    {
        Debug.Log("Player Die ! ");

        //monster �±׸����� ��� ���� ������Ʈ�� ã�ƿ�
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //foreach(GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        OnPlayerDie();

        // GameManage ��ũ��Ʈ�� IsGameOver ������Ƽ ���� ����
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;

    }
    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}
