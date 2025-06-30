using UnityEngine;

public enum PickupType
{
    None,
    Bomb,
    Key,
    Heart
}


[CreateAssetMenu(fileName = "PickupItem", menuName = "SO/ItemData/PickupData")]
public class PickupItemData : ItemData
{
    [SerializeField] private PickupType pickupType;

    public PickupType PickupType => pickupType;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetItemType(ItemType.Pickup);
    }

    public void AddPickup(Player player)
    {
        // �÷��̾��� �κ��丮�� �Ⱦ������� �߰��ϴ� �ڵ�
    }
}
