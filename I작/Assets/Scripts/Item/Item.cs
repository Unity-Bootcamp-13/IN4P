using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemRepositorySO itemRepositorySO;
    public int itemId;
    private ItemModel itemModel;
    private List<EffectModel> effectModels;
    public Sprite itemIcon;

    public List<EffectModel> EffectModels => effectModels;

    private void Start()
    {
        itemModel = itemRepositorySO.itemRepository.GetItemModelById(itemId);        
        effectModels = itemRepositorySO.itemRepository.GetEffectModelsByItemId(itemId);
                
        Sprite[] sprites = Resources.LoadAll<Sprite>(itemModel.itemImagePath);
        itemIcon = Array.Find(sprites, s => s.name == itemId.ToString());

        GetComponent<SpriteRenderer>().sprite = itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.tag == "Player")
        {
            Debug.Log("player °¨Áö");
            collision.GetComponent<TestPlayer>().ApplyEffect(this);

            Destroy(gameObject);
        }
    }   
}