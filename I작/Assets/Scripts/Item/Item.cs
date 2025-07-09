using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemId;    
    public string spritePath;
    private Sprite itemIcon;

    private void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
        itemIcon = Array.Find(sprites, s => s.name == itemId.ToString());
        GetComponent<SpriteRenderer>().sprite = itemIcon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if ((player.stats.CurrentHp >= player.stats.Max_Hp) &&
                (itemId == 1003 || itemId == 1004))
            {
                return;
            }
            else
            {
                player.AcquireItem(itemId, itemIcon);
                Destroy(gameObject);
            }
        }
    }
}