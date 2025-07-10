using UnityEngine;

public class GoldChest : MonoBehaviour
{
    [SerializeField] ItemServiceSO itemServiceSO;
    private ItemGenerator itemGenerator;

    public Animator chestAnimator;

    int openGoldChest;

    private void Start()
    {
        itemGenerator = new ItemGenerator(itemServiceSO.itemService);
        openGoldChest = Animator.StringToHash("OpenChest");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if(player.stats.KeyCount > 0)
            {
                player.stats.ChangeKey(-1);
                chestAnimator.SetTrigger(openGoldChest);
            }
        }
    }

    private void OpenTreasureChest()
    {
        SoundManager.Instance.PlaySFX(SFX.ChestOpen);
        int random = Random.Range(0, 5);

        for (int j = 0; j < random; j++)
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            GameObject itemGo = itemGenerator.GetRandomPickupItem(transform.position + randomPosition);

            if (itemGo.GetComponent<Item>().itemId == 1004)
            {
                Destroy(itemGo);
            }
        }

        int randomGoodItem = Random.Range(0, 10);
        if(randomGoodItem >= 9)
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            itemGenerator.GetRandomPassiveItem(transform.position + randomPosition);
        }
        Destroy(gameObject);
    }
}
