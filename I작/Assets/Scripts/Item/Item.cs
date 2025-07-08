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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<TestPlayer>().AcquireItem(itemId);

            Destroy(gameObject);
        }
    }
}