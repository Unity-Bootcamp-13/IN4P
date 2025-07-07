using TMPro;
using UnityEngine;

public class SecretDoorController : MonoBehaviour
{
    public Door secretDoor;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            secretDoor.OpenDoor();
            transform.GetChild(0).gameObject.SetActive(false);
            secretDoor.portalCollider.isTrigger = true;

            Vector3 pushDirection = collision.contacts[0].normal;

            collision.gameObject.transform.position -= pushDirection * 0.5f;

        }
    }
}
