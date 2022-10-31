using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //ui사용을 위한 네임스페이스 선언
using UnityEngine.Events; //UnityEvents API를 사용하기 위한 네임스페이스 선언
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    public UnityAction action;

    void Start()
    {
        //UnityAcion을 이용한 연결방식
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        //무명 메소드를 활용한 이벤트 연결 방식
        optionButton.onClick.AddListener(delegate{ OnButtonClick(optionButton.name);});

        // 람다식을 활요한 이벤트 연결 방식
        shopButton.onClick.AddListener(()=> OnButtonClick(shopButton.name));
    }
    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }

    public void OnStartClick()
    {   
        //LoadScene > 씬의 이름 또는 인덱스 번호로 씬로드
        SceneManager.LoadScene("Level_01");
        SceneManager.LoadScene("Play", LoadSceneMode.Additive); //.Additve 기존 씬 유지 추가로 새로운 씬 로드
                                                                //.Single 기존 씬 삭제 후 새로운 씬 로드      
    }
}
