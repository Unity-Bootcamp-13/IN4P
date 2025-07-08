using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private ItemServiceSO itemServiceSO;
    public CharacterData characterData;
    
    private TestStats stats;
    private TestStats oldStats;
    private List<int> passiveItems = new List<int>();
    private int activeItem = -1; // 액티브 아이템 없는 상태
    public int currentGauge;

    private void Awake()
    {
        stats = new TestStats
        (
            0,
            0,
            characterData.PlayerHp,
            characterData.Atk,
            characterData.AtkSpeed,
            characterData.Speed,
            characterData.AtkRange,
            characterData.ProjectileSpeed
        );
    }


    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0).normalized;
        transform.position += direction * stats.Speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseActiveItem();
        }
    }

    public void AcquireItem(int id)
    {
        List<StatModifier> statModifiers = itemServiceSO.itemService.GetStatModifier(id);
        ItemType itemType = itemServiceSO.itemService.GetItemType(id);

        if (itemType == ItemType.Passive || itemType == ItemType.Pickup)
        {
            passiveItems.Add(id);

            for (int i = 0; i < statModifiers.Count; i++)
            {
                stats = stats.Apply(statModifiers[i]);
            }
        }
        else if (itemType == ItemType.Active)
        {
            DropActiveItem();
            activeItem = id;
            currentGauge = itemServiceSO.itemService.GetItemGauge(id);
        }
    }

    private void DropActiveItem()
    {
        if (activeItem < 0)
            return;

        GameObject prefab = Resources.Load<GameObject>("Prefab/ItemPrefab");
        GameObject itemGo = GameObject.Instantiate(prefab, transform.position + Vector3.down * 2f, Quaternion.identity);
        var item = itemGo.GetComponent<Item>();
        item.itemId = activeItem;
        item.spritePath = itemServiceSO.itemService.GetSpritePath(activeItem);

        activeItem = -1;
    }

    private void UseActiveItem()
    {
        if (activeItem < 0)
        {
            Debug.Log("액티브 아이템 없음");
            return;
        }

        if (currentGauge < itemServiceSO.itemService.GetItemGauge(activeItem))
        {
            Debug.Log("게이지 부족");
            return;
        }

        oldStats = stats;
        
        List<StatModifier> statModifiers = itemServiceSO.itemService.GetStatModifier(activeItem);

        for (int i = 0; i < statModifiers.Count; i++)
        {
            stats = stats.Apply(statModifiers[i]);
        }

        currentGauge = 0;
        Debug.Log("사용");
    }

    public void RevertStats()
    {
        if (oldStats != null)
        {
            stats = oldStats;
            oldStats = null;
        }
    }
}
