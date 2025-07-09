using UnityEngine;

public class BrownChest : MonoBehaviour
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
            chestAnimator.SetTrigger(openGoldChest);
        }
    }

    private void OpenTreasureChest()
    {
        SoundManager.Instance.PlaySFX(SFX.ChestOpen);
        int random = Random.Range(0, 3);

        Debug.Log(random);
        for (int j = 0; j < random; j++)
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            GameObject itemGo = itemGenerator.GetRandomPickupItem(transform.position + randomPosition);
            Debug.Log(itemGo.GetComponent<Item>().itemId);
            if (itemGo.GetComponent<Item>().itemId == 1003)
            {
                Destroy(itemGo);
            }
        }
        Destroy(gameObject);
    }
}
