using UnityEngine;

public class GoldChest : MonoBehaviour
{
    public Animator chestAnimator;
    public GameObject[] dropItems;

    int openGoldChest;

    private void Start()
    {
        openGoldChest = Animator.StringToHash("OpenChest");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if(player.stats.KeyCount > 0)
            {
                player.stats.KeyCount--;
                chestAnimator.SetTrigger(openGoldChest);
            }
        }
    }

    private void OpenTreasureChest()
    {
        SoundManager.Instance.PlaySFX(SFX.ChestOpen);
        for (int i = 0; i < dropItems.Length; i++)
        {
            int random = Random.Range(0, 3);

            for (int j = 0; j < random; j++)
            {
                Vector3 randomPosition = Random.insideUnitCircle;
                Instantiate(dropItems[i], transform.position + randomPosition * 1.5f, Quaternion.identity);
            }
        }
                
        Destroy(gameObject);
    }
}
