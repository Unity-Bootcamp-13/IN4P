using UnityEngine;

public class HeartItem : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.currentHp < player.hp)
            {
                player.currentHp++;
                Mathf.Clamp(player.currentHp, 0, player.hp);
                Destroy(gameObject);
            }
        }
    }
}
