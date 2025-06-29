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
        // 플레이어의 인벤토리의 픽업아이템 추가하는 코드
    }
}
