using UnityEngine;

public class DroppedBomb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().bombCount++;

            Destroy(gameObject);
        }
    }
}
