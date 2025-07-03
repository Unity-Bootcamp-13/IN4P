using UnityEngine;

public class DroppedKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().keyCount++;

            Destroy(gameObject);
        }
    }
}
