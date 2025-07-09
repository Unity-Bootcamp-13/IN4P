using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator
{
    public ItemGenerator(ItemService service)
    {
        itemService = service;
        pickupItems = itemService.GetPickupItemId();
        passiveItems = itemService.GetPassiveItemId();
        activeItems = itemService.GetActiveItmeId();
    }

    private ItemService itemService;
    private List<int> pickupItems;
    private List<int> passiveItems;
    private List<int> activeItems;

    private GameObject CreateItem(int itemId, Vector3 pos)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefab/ItemPrefab");
        GameObject itemGo = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        var item = itemGo.GetComponent<Item>();
        item.itemId = itemId;
        item.spritePath = itemService.GetSpritePath(itemId);

        return itemGo;
    }

    public GameObject GetRandomPickupItem(Vector3 pos)
    {
        int random = Random.Range(0, pickupItems.Count);
        return CreateItem(pickupItems[random], pos);
    }

    public GameObject GetRandomPassiveItem(Vector3 pos)
    {
        int random = Random.Range(0, passiveItems.Count);
        return CreateItem(passiveItems[random], pos);
    }

    public GameObject GetRandomActiveItem(Vector3 pos)
    {
        int random = Random.Range(0, activeItems.Count);
        return CreateItem(activeItems[random], pos);
    }
}
