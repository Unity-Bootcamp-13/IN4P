using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Paused : MonoBehaviour
{
    [SerializeField] ItemServiceSO itemserviceSO;
    [SerializeField] Player player;
    [SerializeField] GameObject slotImage;
    [SerializeField] Transform slotInventory;

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
        if (optionImage.activeSelf)
        {
            optionImage.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            optionImage.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        SetPassiveSlot();
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

    private void SetPassiveSlot()
    {
        List<int> passiveItmeIds = player.passiveItems;

        for (int i = 0; i < passiveItmeIds.Count; i++)
        {
            string iconPath = itemserviceSO.itemService.GetSpritePath(passiveItmeIds[i]);
            Sprite iconSprite = Resources.Load<Sprite>(iconPath);

            var go = Instantiate(slotImage, slotInventory);
            go.GetComponent<SpriteRenderer>().sprite = iconSprite;
        }
    }
}
