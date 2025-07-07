using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Paused : MonoBehaviour
{
    public Button optionButton;
    public Button continueButton;
    public Button rerunButton;
    public Button rerunYesButton;
    public Button rerunNoButton;

    public GameObject optionImage;
    public GameObject rerunImage;

    // 일시 정지 여부
    bool isPaused;

    private void Start()
    {
        continueButton.onClick.AddListener(onContinueButton);
        optionButton.onClick.AddListener(onOptionButton);
        rerunButton.onClick.AddListener(onRerunButton);
        rerunYesButton.onClick.AddListener(onRerunYesButton);
        rerunNoButton.onClick.AddListener(onRerunNoButton);
    }

    void Update()
    {

    }

    public void onOptionButton()
    {
        isPaused = true;
        optionImage.SetActive(true);
        Time.timeScale = 0f;
    }

    public void onRerunButton()
    {
        rerunImage.SetActive(true) ;
    }

    public void onRerunYesButton()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    public void onRerunNoButton()
    {
        rerunImage.SetActive(false);
    }

    public void onContinueButton()
    {
        isPaused = false;
        optionImage.SetActive(false);
        Time.timeScale = 1f;
    }
}
