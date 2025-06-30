using UnityEngine;

public enum ItemType
{
    None,
    Passive,
    Active,
    Pickup,
}

public abstract class ItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private ItemType itemType;

    // �б� ���� ������
    public int ItemId => itemId;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite ItemIcon => itemIcon;

    protected void SetItemType(ItemType type)
    {
        itemType = type;
    }

    protected virtual void OnEnable()
    {

    }
}