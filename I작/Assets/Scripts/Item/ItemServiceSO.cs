using UnityEngine;

[CreateAssetMenu(fileName = "ItemServiceSO", menuName = "SO/Service")]
public class ItemServiceSO : ScriptableObject
{
    public ItemService itemService { get; private set; }

    public void Init()
    {
        itemService = new ItemService();
    }
}
