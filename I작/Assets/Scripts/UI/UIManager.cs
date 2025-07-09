using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Image[] heartImage;
    public Sprite FullHeartSprite;
    public Sprite HalfHeartSprite;
    public Sprite BlacnkHeartSprite;

    public Text keyCount;
    public Text bombCount;

    public GameObject PlayerHp;
    public GameObject heartPrefab;

    public int statehp;

    public void Awake()
    {
    }

    private void OnEnable()
    {
        Stats.OnChanged += HandleStatChanged;

    }

    private void OnDisable()
    {
        Stats.OnChanged -= HandleStatChanged;
    }

    private void HandleStatChanged(StatType type, int value)
    {
        switch (type)
        {
            case StatType.MaxHP: UpdateHP(value); break;
            case StatType.CurrentHP: UPdateHearts(value); break;
            case StatType.Bomb: UpdateBombCount(value); break;
            case StatType.Key: UpdateKeyCount(value); break;
        }
    }

    private void HeartInstantiate(int count)
    {
        for (int i = 0; i < count; i++)
        {

            GameObject heart = Instantiate(heartPrefab, PlayerHp.transform);
        }
    }

    private void UpdateHP(int maxhp)
    {
        int hp = maxhp / 2;
        HeartInstantiate(hp);
    }

    public void UPdateHearts(int currenthp)
    {
        //int hp = 

        //heartImage[i].sprite = FullHeartSprite;
        //heartImage[i].sprite = HalfHeartSprite;
        //heartImage[i].sprite = BlacnkHeartSprite;
    }

    public void UpdateBombCount(int count)
    {

    }

    public void UpdateKeyCount(int count)
    {

    }
}
