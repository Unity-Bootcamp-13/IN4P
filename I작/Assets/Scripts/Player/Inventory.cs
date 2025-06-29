using System.Collections.Generic;
using Unity.VisualScripting;

public class Inventory
{
    private List<PassiveItemData> passiveItemList = new List<PassiveItemData>();
    private ActiveItemData equippedActiveItem;
    private int activeGauge;
    public int bombCount;
    public int keyCount;


    public void AddPassiveItem(PassiveItemData item)
    {
        passiveItemList.Add(item);
    }

    public void EquipActiveItem(ActiveItemData item)
    {
        equippedActiveItem = item;
        activeGauge = item.Gauge;
    }

    public void UseActiveItem(Player player)
    {
        if (equippedActiveItem == null)
            return;

        if (activeGauge >= equippedActiveItem.Gauge)
        {
            equippedActiveItem.activeAction?.Invoke(player);
            activeGauge = 0;
        }
    }
}