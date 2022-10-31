using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������̼� ����� ����ϱ� ���� �߰��ϴ� ���ӽ����̽�
using UnityEngine.AI;


public class MonsterCtrl : MonoBehaviour
{   
    // component ĳ�� ó��
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    //����ȿ�� ������
    private GameObject bloodEffect;
    //������ ü��
    private int hp = 100;


    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    // ������ ���� ����
    public State state = State.IDLE;

    // ���� �����Ÿ�
    public float traceDist = 10.0f;

    // ���� �����Ÿ�
    public float attackDist = 2.0f;

    // ������ ��� ����
    public bool isDie = false;

    void OnEnable()
    {   
        //PlayerCtrl�� OnPlayerDie�� MonsterCtrl�� OnPlayerDie����
        //�̺�Ʈ ����, �̺�Ʈ�� ����� Ŭ������.�̺�Ʈ�� += �߻��� ȣ���� �Լ�
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        
        //���� ����� ��ġ�� �����ϸ� �ٷ� �߰� ����
        //agent.destination = playerTr.position;

        //������ ���¸� üũ�ϴ� �ڷ�ƾ�Լ� 
        StartCoroutine(CheckMonsterState());
        //���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(MonsterAction());
    }

    void OnDisable()
    {
        //�̺�Ʈ ����, �̺�Ʈ�� ����� Ŭ������.�̺�Ʈ�� -= �߻��� ȣ���� �Լ�
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    void Awake()
    {
        //monster�� transform�Ҵ�
        monsterTr = GetComponent<Transform>();

        //player�� transform�Ҵ�
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        //NavMeshAgent component �Ҵ�
        agent = GetComponent<NavMeshAgent>();
        //navmeshagent�� �ڵ�ȸ�� ��� ��Ȱ��ȭ
        agent.updateRotation = false;

        //Animatior component �Ҵ�
        anim = GetComponent<Animator>();

        //blood ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    void Update()
    {
        //���������� ���� �Ÿ��� ȸ�� ���� �Ǵ�
        if(agent.remainingDistance >= 2.0f)
        {
            //������Ʈ�� �̵� ����
            Vector3 direction = agent.desiredVelocity;
            //ȸ���� ����
            Quaternion rot = Quaternion.LookRotation(direction);
            //���� �������� �Լ��� �ε巯�� ȸ��ó��
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation,
                                                    rot,
                                                    Time.deltaTime * 10.0f);
        }
    }

 

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 0.3���� ����(���) �ϴ� ���� ������� �޽��� ������ �纸
            yield return new WaitForSeconds(0.3f);
            //������ ���� üũ
            if(state == State.DIE) yield break;
            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            // ���� �����Ÿ� ������ ���Դ��� Ȯ��.
            if(distance <= attackDist)
            {
                state = State.ATTACK;
            }
            // ���� �����Ÿ� ������ ���Դ��� Ȯ��.
            else if(distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE����
                case State.IDLE:
                    // ��������
                    agent.isStopped = true;
                    // animator�� istrace ������ false�� ����
                    anim.SetBool(hashTrace, false);
                    break;

                //��������
                case State.TRACE:
                    // ���� ����� ��ǥ�� �̵� ����.
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                    // animator�� istrace ������ true�� ����
                    anim.SetBool(hashTrace, true);

                    // animator�� isAttack ������ false�� ����
                    anim.SetBool(hashAttack, false);
                    break;

                // ���ݻ���
                case State.ATTACK:
                    // animator�� isAttack ������ true�� ����
                    anim.SetBool(hashAttack, true);
                    break;

                // ���
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    // collider ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;

                    //�����ð� ����� ������Ʈ Ǯ������ ȯ��
                    yield return new WaitForSeconds(3.0f);

                    //��� �� �ٽ� ����� ���� ���� hp�� �ʱ�ȭ
                    hp = 100;
                    isDie = false;

                    //������ Collider Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = true;
                    //���͸� ��Ȱ��ȭ
                    this.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            //�浹 �Ѿ� ����
            Destroy(collision.gameObject);
            //�ǰ� �ִϸ��̼� ����
            anim.SetTrigger(hashHit);
            

            //�Ѿ��� �浹 ����
            Vector3 pos = collision.GetContact(0).point;
            //�Ѿ��� �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            //���� ȿ���� �����ϴ� �Լ� ȣ��'
            ShowBloodEffect(pos, rot);

            hp -= 10;
            if (hp <= 0)
            {
                state = State.DIE;
                GameManager.instance.DisplayScore(50);
            }
        }
    }
   void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.gameObject.name);
    }

    void ShowBloodEffect(Vector3 pos , Quaternion rot)
    {
        //Instantiate(������ü, ��ġ , ����, �θ���� ������Ʈ)
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        //1�ʵ� blood gameObejcet �ı�
        Destroy(blood, 1.0f);
    }


    void OnDrawGizmos()
    {
        //���� �����Ÿ� ǥ��
        if(state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        //���� �����Ÿ� ǥ��
        if(state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }    

    //������ ���¸� �����ϰ� �ൿ�ϴ� �ڷ�ƾ�Լ� ���� �� NavMeshAgent component�� ����
    void OnPlayerDie()
    {
        //������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ��� ���� ����
        StopAllCoroutines();

        //������ �����ϰ� �ִϸ��̼��� ����
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }

    //raycast�� ����� ��������  ������ logic
    public void OnDamage(Vector3 pos,Vector3 normal)
    {   
        //�ǰ� �ִϸ��̼� ���
        anim.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);

        ShowBloodEffect(pos, rot);

        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;
            //���� ����� 50�� �߰�
            GameManager.instance.DisplayScore(50);
        }
    }
}
