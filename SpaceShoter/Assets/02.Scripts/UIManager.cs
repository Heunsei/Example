using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //ui����� ���� ���ӽ����̽� ����
using UnityEngine.Events; //UnityEvents API�� ����ϱ� ���� ���ӽ����̽� ����
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    public UnityAction action;

    void Start()
    {
        //UnityAcion�� �̿��� ������
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        //���� �޼ҵ带 Ȱ���� �̺�Ʈ ���� ���
        optionButton.onClick.AddListener(delegate{ OnButtonClick(optionButton.name);});

        // ���ٽ��� Ȱ���� �̺�Ʈ ���� ���
        shopButton.onClick.AddListener(()=> OnButtonClick(shopButton.name));
    }
    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }

    public void OnStartClick()
    {   
        //LoadScene > ���� �̸� �Ǵ� �ε��� ��ȣ�� ���ε�
        SceneManager.LoadScene("Level_01");
        SceneManager.LoadScene("Play", LoadSceneMode.Additive); //.Additve ���� �� ���� �߰��� ���ο� �� �ε�
                                                                //.Single ���� �� ���� �� ���ο� �� �ε�      
    }
}
