using System.Collections.Generic;
using System.Linq;


public class ItemService
{
    private ItemRepository itemRepository;

    public ItemService()
    {
        itemRepository = new ItemRepository();
    }

    public List<int> GetAllItemId()
    {
        List<ItemData> itemDatas = itemRepository.GetAllItemDatas();
        return itemDatas.Select(item => item.itemID).ToList();
    }

    public List<int> GetPickupItemId()
    {
        List<ItemData> itemDatas = itemRepository.GetAllItemDatas();
        
        return itemDatas
        .Where(item => item.itemType == ItemType.Pickup)
        .Select(item => item.itemID)
        .ToList();
    }

    public List<int> GetPassiveItemId()
    {
        List<ItemData> itemDatas = itemRepository.GetAllItemDatas();
        
        return itemDatas
        .Where(item => item.itemType == ItemType.Passive)
        .Select(item => item.itemID)
        .ToList();
    }

    public List<int> GetActiveItmeId()
    {
        List<ItemData> itemDatas = itemRepository.GetAllItemDatas();
        
        return itemDatas
        .Where(item => item.itemType == ItemType.Active)
        .Select(item => item.itemID)
        .ToList();
    }

    public string GetSpritePath(int id)
    {
        return itemRepository.GetItemDataById(id).itemImagePath;
    }

    public ItemType GetItemType(int id)
    {
        return itemRepository.GetItemDataById(id).itemType;
    }

    public int GetItemGauge(int id)
    {
        return itemRepository.GetGaugeDataById(id).value;
    }

    public List<StatModifier> GetStatModifier(int id)
    {
        ItemData itemData = itemRepository.GetItemDataById(id);
        List<EffectData> effectData = itemRepository.GetEffectDatasByItemId(id);
        List<StatModifier> statModifiers = new List<StatModifier>();

        for (int i = 0; i < effectData.Count; i++)
        {
            StatModifier statModifier = new StatModifier(effectData[i].modifyTarget, effectData[i].modifyType, effectData[i].value);
            statModifiers.Add(statModifier);
        }

        return statModifiers;
    }
}
