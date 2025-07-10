
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public Sprite FullHeartSprite;
    public Sprite HalfHeartSprite;
    public Sprite EmptyHeartSprite;

    public Text keyCount;
    public Text bombCount;

    public GameObject PlayerHp;
    public GameObject heartPrefab;

    public Button gameoverBackMainButton;
    public Button gameoverRerunButton;
    public Button gameclearRerunButton;

    public GameObject GameOver;
    public GameObject GameClear;

    public Slider bossHpSlider;
    public GameObject bossHpParent;

    private float bossMaxHp;
    public int statehp = 0;
    public int statecurrenthp = 0;

    private void Awake()
    {
        GameOver.SetActive(false);
        GameClear.SetActive(false);
        gameoverBackMainButton.onClick.AddListener(onBackMenu);
        gameoverRerunButton.onClick.AddListener(ReLoadGameScene);
        gameclearRerunButton.onClick.AddListener(onBackMenu);
    }

    private void OnEnable()
    {
        Stats.OnChanged += HandleStatChanged;
        Monstro.onBossHpSlider += BossCurrentHp;
        Player.OnPlayerDead += PlayerDead;
        Monstro.onDeath += BossDead;
    }

    private void OnDisable()
    {
        Stats.OnChanged -= HandleStatChanged;
        Monstro.onBossHpSlider -= BossCurrentHp;
        Player.OnPlayerDead -= PlayerDead;
        Monstro.onDeath -= BossDead;

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

    private void HeartDestory(int count)
    {
        var destoyhurt = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            destoyhurt.Add(PlayerHp.transform.GetChild(i).gameObject);
        }

        foreach (var hurt in destoyhurt)
        {
            Destroy(hurt);
        }
    }

    private void UpdateHP(int maxhp)
    {
        int hp = maxhp / 2;

        if (statehp < hp)
        {
            HeartInstantiate(hp-statehp);
        }
        else if (statehp == hp)
        {
            return;
        }
        else
        {
            int minushp = statehp - hp;
            HeartDestory(minushp);
        }

        statehp = hp;
    }
    public void UPdateHearts(int currenthp)
    {
        int slotCount = statehp;
        int fullCount = currenthp / 2;
        bool hasHalf = (currenthp % 2) == 1;

        for (int i = 0; i < slotCount; i++)
        {
            Image img = PlayerHp.transform
            .GetChild(i)
            .GetComponent<Image>();

            if (i < fullCount)
            {
                img.sprite = FullHeartSprite;
            }
            else if (i == fullCount && hasHalf)
            {
                img.sprite = HalfHeartSprite;
            }
            else
            {
                img.sprite = EmptyHeartSprite;
            }
        }

        statecurrenthp = currenthp;


    }

    public void UpdateBombCount(int count)
    {
        bombCount.text = "x " + count.ToString();
    }

    public void UpdateKeyCount(int count)
    {
        keyCount.text = "x " + count.ToString();
    }

    public void BossCurrentHp(float currentHP)
    {
        if (bossMaxHp == 0f)
        {
            bossMaxHp = currentHP;
            bossHpSlider.maxValue = bossMaxHp;
        }

        bossHpParent.SetActive(true);
        bossHpSlider.value = currentHP;

        if (currentHP <= 0f)
            bossHpParent.SetActive(false);
    }

    private void BossDead()
    {
        Time.timeScale = 0f;
        GameClear.SetActive(true);
    }

    private void PlayerDead(int playerhp)
    {
        if (playerhp == 0)
        {
            Time.timeScale = 0f;
            GameOver.SetActive(true);
        }
    }

    private void onBackMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    private void ReLoadGameScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
