using UnityEngine;

public class HalfHeart : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.currentHp < player.Max_hp)
            {
                player.currentHp ++;
                Mathf.Clamp(player.currentHp, 0, player.Max_hp);
                Destroy(gameObject);
            }
        }
    }
}
