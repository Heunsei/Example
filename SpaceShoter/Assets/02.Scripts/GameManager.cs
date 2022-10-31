using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TextMesh Pro ���� ������Ʈ�� ����
using TMPro;

public class GameManager : MonoBehaviour
{
    //monster�� ������ ��ġ�� ������ ListŸ�� ����
    public List<Transform> points = new List<Transform>();

    //���͸� �̸� ������ ������ ����Ʈ �ڷ���
    public List<GameObject> monsterPool = new List<GameObject> ();

    //������Ʈ Ǯ �� ������ ������ �ִ� ����
    public int maxMonsters = 10;

    //���� �������� ������ ����
    public GameObject monster;

    //������ ���� ����
    public float createTime = 3.0f;

    //������ ���� ���θ� ���� ��� �Լ�
    public bool isGameOver;

    //������ ���� ���θ� ������ ������Ƽ
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {   
                //������ �Լ� ����.
                CancelInvoke("CreateMonster");
            }
        }
    }

    //��������� ���� �ؽ�Ʈ ���� ����
    public TMP_Text scoreText;
    //�ְ� ���� ���
    private int totScore = 0;

    //�̱��� �ν��Ͻ� ����
    //static ���� �����Ͽ� �޸𸮿� ����
    //�ܺ������� �����ϵ��� public
    public static GameManager instance = null;

    //��ũ��Ʈ�� ����Ǹ� ������� ȣ��Ǵ� ����Ƽ �̺�Ʈ �Լ�
    void Awake()
    {   

        if(instance == null)
        {
            instance = this;
        }
        //instance�� �Ҵ�� Ŭ���� �ν��Ͻ��� �ٸ� ��� ���Ӱ� ������ Ŭ������ �ǹ�
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        //�ٸ� ������ �Ѿ���� �������� �ʰ� ����
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //���� ������Ʈ Ǯ ����
        CreateMonsterPool();

        // SpawnPointGroup ���ӿ�����Ʈ�� Transform ������Ʈ ����
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //SpawnPointGroup ������ �ִ� ��� ���ϵ� ���ӿ�����Ʈ�� Transform ������Ʈ ����
        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);
        foreach(Transform point in spawnPointGroup)
        {
            points.Add(point);
        }


        //������ �ð� �������� �Լ��� ȣ��
        //Invoke("ȣ���� �Լ�", ���_�ð�);
        //InvokeRepeating("ȣ���� �Լ�", ���_�ð�, ȣ��_����);

        InvokeRepeating("CreateMonster", 2.0f, createTime);

        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DisplayScore(0);
  
    }
    //���� ����
        void CreateMonster()
        {
            //������ �ұ�Ģ�� ���� ��ġ ����
            int idx = Random.Range(0,points.Count);
            //���� ������ ����
            //Instantiate(monster, points[idx].position, points[idx].rotation );
            //������Ʈ Ǯ���� ���� ����
            GameObject _monster = GetMonsterInPool();
            //������ ������ ��ġ�� ȸ���� ����
            _monster?.transform.SetPositionAndRotation(points[idx].position,
                                                   points[idx].rotation);
            // ������ ���͸� Ȱ��ȭ
            _monster.SetActive(true);
            
        }

    void CreateMonsterPool()
    {
        for(int i =0; i < maxMonsters; i++)
        {
            //���� ����
            var _monster = Instantiate<GameObject>(monster);
            //������ �̸� ����
            _monster.name = $"Monster_i{i:00}";
            //���� ��Ȱ��ȭ
            _monster.SetActive(false);

            //������ ���͸� ������Ʈ Ǯ�� �߰�
            monsterPool.Add(_monster);
        }
    }

    //object pool���� ��� ������ ���͸� ������ ��ȯ�ϴ� �Լ�
    public GameObject GetMonsterInPool()
    {
        //������Ʈ Ǯ�� ó������ ������ ��ȸ
        foreach(var _monster in monsterPool)
        {
            //��Ȱ��ȭ ���η� ��� ������ ���͸� �Ǵ�
            if(_monster.activeSelf == false)
            {
                //���� ��ȯ
                return _monster;
            }
        }
        return null;
    }

    //���� ���� �� ǥ��
    // PlayerPrefs�� ���ȼ� 0;
    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totScore:#,##0} </color>";
        //score save
        PlayerPrefs.SetInt("TOT_SCORE",totScore);
    }

}
