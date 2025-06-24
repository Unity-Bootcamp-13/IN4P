using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    public string gainComment;
}