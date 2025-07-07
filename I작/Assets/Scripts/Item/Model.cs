using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    None,
    Pickup,
    Passive,
    Active
}

public enum EffectType
{
    None,
    GainPickup,
    ModifyStat
}

public enum EffectTarget
{
    None,
    BombCount,
    KeyCount,
    Atk,
    AtkRate,
    AtkSpeed,
    AtkSpeedRate,
    AtkRange,
    AtkRangeRate,
    Speed,
    ProjectileSpeed,
    MaxHp,
    CurrentHp
}

[Serializable]
public class ItemModel
{
    public int itemID;
    public string itemName;
    public string itemImagePath;
    public ItemType itemType;
    public int gauge;
}

[Serializable]
public class ItemModelList
{
    public List<ItemModel> itemModels;
}

[Serializable]
public class EffectModel
{
    public int effectID;
    public int itemID;
    public EffectType effectType;
    public EffectTarget target;
    public float value;
}

public class EffectModelList
{
    public List<EffectModel> effectModels;
}

public class ItemRepository
{
    private readonly string ItemDataPath = "Data/ItemData";
    private readonly string EffectDataPath = "Data/EffectData";

    public List<ItemModel> ItemModels => itemModels;
    public List<EffectModel> EffectModels => effectModels;

    private List<ItemModel> itemModels;
    private List<EffectModel> effectModels;

    public void Load()
    {
        TextAsset itemTextAsset = Resources.Load<TextAsset>(ItemDataPath);
        TextAsset effectTextAsset = Resources.Load<TextAsset>(EffectDataPath);

        ItemModelList itemModelList = JsonUtility.FromJson<ItemModelList>(itemTextAsset.text);
        EffectModelList effectModelList = JsonUtility.FromJson<EffectModelList>(effectTextAsset.text);

        itemModels = itemModelList.itemModels;
        effectModels = effectModelList.effectModels;
    }

    public List<int> GetAllId()
    {
        return itemModels.Select(item => item.itemID).ToList();
    }

    public ItemModel GetItemModelById(int id)
    {
        return itemModels.Find(item => item.itemID == id);
    }

    public List<EffectModel> GetEffectModelsByItemId(int itemId)
    {
        return effectModels.FindAll(effect => effect.itemID == itemId);
    }
}