using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.currentHp < player.hp)
            {
                player.currentHp += 2;
                Mathf.Clamp(player.currentHp, 0, player.hp);
                Destroy(gameObject);
            }
        }
    }
}
