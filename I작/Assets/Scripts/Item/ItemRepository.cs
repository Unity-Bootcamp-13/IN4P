using System;
using System.Collections.Generic;
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

public enum ModifyTarget
{
    None,
    BombCount,
    KeyCount,
    Atk,
    AtkSpeed,
    AtkRange,
    Speed,
    ProjectileSpeed,
    MaxHp,
    CurrentHp
}

public enum ModifyType
{
    Noe,
    Addtion,
    Multiplication
}

[Serializable]
public class ItemData
{
    public int itemID;
    public string itemName;
    public string itemImagePath;
    public ItemType itemType;
}

[Serializable]
public class ItemDataList
{
    public List<ItemData> itemDatas;
}

[Serializable]
public class GaugeData
{
    public int gaugeID;
    public int itemID;
    public int value;
}

[Serializable]
public class GauGeDataList
{
    public List<GaugeData> gaugeDatas;
}

[Serializable]
public class EffectData
{
    public int effectID;
    public int itemID;
    public EffectType effectType;
    public ModifyTarget modifyTarget;
    public ModifyType modifyType;
    public float value;
}

public class EffectDataList
{
    public List<EffectData> effectDatas;
}

public class ItemRepository
{
    private readonly string ItemDataPath = "Data/ItemData";
    private readonly string GaugeDataPath = "Data/GaugeData";
    private readonly string EffectDataPath = "Data/EffectData";

    public List<ItemData> ItemDatas => itemDatas;
    public List<GaugeData> GaugeDatas => gaugeDatas;
    public List<EffectData> EffectDatas => effecDatas;

    private List<ItemData> itemDatas;
    private List<GaugeData> gaugeDatas;
    private List<EffectData> effecDatas;

    public ItemRepository()
    {
        Load();
    }

    public void Load()
    {
        TextAsset itemTextAsset = Resources.Load<TextAsset>(ItemDataPath);
        TextAsset gaugeTextAsset = Resources.Load<TextAsset>(GaugeDataPath);
        TextAsset effectTextAsset = Resources.Load<TextAsset>(EffectDataPath);

        ItemDataList itemDataList = JsonUtility.FromJson<ItemDataList>(itemTextAsset.text);
        GauGeDataList gaugeDataList = JsonUtility.FromJson<GauGeDataList>(gaugeTextAsset.text);
        EffectDataList effectDataList = JsonUtility.FromJson<EffectDataList>(effectTextAsset.text);

        itemDatas = itemDataList.itemDatas;
        gaugeDatas = gaugeDataList.gaugeDatas;
        effecDatas = effectDataList.effectDatas;
    }   

    public List<ItemData> GetAllItemDatas()
    {
        return itemDatas;
    }

    public ItemData GetItemDataById(int id)
    {
        return itemDatas.Find(item => item.itemID == id);
    }

    public GaugeData GetGaugeDataById(int id)
    {
        return gaugeDatas.Find(gauge => gauge.itemID == id);
    }

    public List<EffectData> GetEffectDatasByItemId(int itemId)
    {
        return effecDatas.FindAll(effect => effect.itemID == itemId);
    }
}