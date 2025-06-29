using UnityEngine;

public class GenericItems : MonoBehaviour
{
    public ItemData itemData;
    public SpriteRenderer thisSpriteRenderer;

    void Start()
    {
        thisSpriteRenderer.sprite = itemData.ItemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().AcquireItem(itemData);
            Destroy(gameObject);
        }
    }
}