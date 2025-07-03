using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetPosition;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = targetPosition.position;
        }
    }
}