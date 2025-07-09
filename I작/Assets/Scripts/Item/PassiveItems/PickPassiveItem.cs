using UnityEngine;

public enum TargetPickup
{
    Key,
    Bomb
}

public class PickPassiveItem : MonoBehaviour
{
    public Sprite itemIcon;

    public TargetPickup targetPickup;
    public int valueToApply;


    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySFX(SFX.PassiveItem);
        if (collision.tag == "Player")
        {
            ApplyEffect(collision.GetComponent<Player>());
            
            //collider.GetComponent<Player>().ApplyItem(itemIcon);

            Destroy(gameObject);
        }
    }

    private void ApplyEffect(Player player)
    {
        switch(targetPickup)
        {
            case TargetPickup.Key:
                player.keyCount += valueToApply;
                break;
            case TargetPickup.Bomb:
                player.bombCount += valueToApply;
                break;
            default:
                break;
        }
    }
}
