using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;
    public GameObject timeTextSetOff;

    private float surviveTime;
    private bool isGameover;

    void Start()
    {
        surviveTime = 0;
        isGameover = false;
    }

    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time : " + (int)surviveTime;
        }
        else
        {       
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("R");
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);
        timeTextSetOff.SetActive(false);

        float bestTime = PlayerPrefs.GetFloat("BestTime");
        if (surviveTime > bestTime)
        {
            //최고 기록값을 현재 생존시간 값으로 변경
            bestTime = surviveTime;

            PlayerPrefs.SetFloat("BestTime",bestTime);
        }
        recordText.text = "Best Time : " + (int)bestTime;
    }
}
