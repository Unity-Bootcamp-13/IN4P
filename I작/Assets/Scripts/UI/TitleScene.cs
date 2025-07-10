using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Image[] changeImage;

    public Button startButton;
    public Image fadeOutImage;


    public Button exitButton;
    public Button exitYesButton;
    public Button exitNoButton;
    public GameObject exitImage;

    private void Awake()
    {
        fadeOutImage.enabled = false;
        exitImage.SetActive(false);

        startButton.onClick.AddListener(onStartButton);
        exitButton.onClick.AddListener(onExitButton);
        exitYesButton.onClick.AddListener(onExitYesButton);
        exitNoButton.onClick.AddListener(onExitNoButton);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onStartButton();   
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onExitNoButton();
        }
    }

    public void onStartButton()
    {
        StartCoroutine(FadoutLoadScene());
    }

    IEnumerator FadoutLoadScene()
    {
        fadeOutImage.enabled = true;

        float fadeTime = 1.0f;
        float elaspedTime = 0f;

        while (elaspedTime < fadeTime)
        {
            elaspedTime += Time.deltaTime;
            fadeOutImage.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elaspedTime / fadeTime));
            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }

    public void onExitButton()
    {
        exitImage.SetActive(true);
    }

    public void onExitYesButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void onExitNoButton()
    {
        exitImage.SetActive(false);
    }
}
