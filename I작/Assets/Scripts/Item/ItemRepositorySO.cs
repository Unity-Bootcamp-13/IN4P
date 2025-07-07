using UnityEngine;

[CreateAssetMenu(fileName = "RepositorySO", menuName = "RepositorySO")]
public class ItemRepositorySO : ScriptableObject
{
    public ItemRepository itemRepository { get; private set; }

    public void Init()
    {
        itemRepository = new ItemRepository();
        itemRepository.Load();
    }
}
