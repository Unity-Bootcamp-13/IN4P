using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Option : MonoBehaviour
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

    private int poolSize = 18;
    private List<GameObject> slotPool = new List<GameObject>();

    // 일시 정지 여부
    bool isPaused;

    public bool isBrimstone;

    private void Start()
    {
        continueButton.onClick.AddListener(onContinueButton);
        optionButton.onClick.AddListener(onOptionButton);
        rerunButton.onClick.AddListener(onRerunButton);
        rerunYesButton.onClick.AddListener(onRerunYesButton);
        rerunNoButton.onClick.AddListener(onRerunNoButton);

        player.transform.GetChild(0).gameObject.GetComponent<Attack>().acquireBrimstone += BrimstoneFlag;
        Debug.Log("start");
        for(int i=0;i<poolSize;i++)
        {
            var go = Instantiate(slotImage, slotInventory);
            go.SetActive(false);
            slotPool.Add(go);
        }
    }

    void Update()
    {

    }

    public void onOptionButton()
    {
        optionImage.SetActive(true);
        Time.timeScale = 0f;
        optionButton.enabled = false;
        isPaused = true;

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
        optionButton.enabled = true;
        Time.timeScale = 1f;
    }

    private void SetPassiveSlot()
    {
        List<int> passiveItemIds = player.passiveItems;
        List<int> distinctNumbers = passiveItemIds.Distinct().ToList();

        int j = 0;

        for (int i = 0; i < distinctNumbers.Count; i++)
        {
            if (j >= 17)
                break;

            if (itemserviceSO.itemService.GetItemType(distinctNumbers[i]) == ItemType.Pickup)
            {
                continue;
            }
            string iconPath = itemserviceSO.itemService.GetSpritePath(distinctNumbers[i]);
            Sprite[] sprites = Resources.LoadAll<Sprite>(iconPath);
            Sprite itemIcon = Array.Find(sprites, s => s.name == distinctNumbers[i].ToString());


            var go = slotPool[j];
            go.GetComponent<Image>().sprite = itemIcon;
            go.SetActive(true);
            j++;
        }


        if (isBrimstone)
        {
            Sprite[] brimstoneSprites = Resources.LoadAll<Sprite>("ItemTexture/Item");
            Sprite brimstoneIcon = Array.Find(brimstoneSprites, s => s.name == 118.ToString());

            var brimstoneGo = slotPool[j];

            brimstoneGo.GetComponent<Image>().sprite = brimstoneIcon;
            brimstoneGo.SetActive(true);
        }
    }

    private void BrimstoneFlag()
    {
        isBrimstone = true;
    }
}
