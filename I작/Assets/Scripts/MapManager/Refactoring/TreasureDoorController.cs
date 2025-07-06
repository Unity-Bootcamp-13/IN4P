using TMPro;
using UnityEngine;

public class TreasureDoorController : MonoBehaviour
{
    public Door treasureDoor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().keyCount >0)
            {
                collision.gameObject.GetComponent<Player>().keyCount--;
                treasureDoor.OpenDoor();
                transform.GetChild(0).gameObject.SetActive(false);
                treasureDoor.portalCollider.isTrigger = true;

                Vector3 pushDirection = collision.contacts[0].normal;

                collision.gameObject.transform.position -= pushDirection * 0.5f;
            }
        }
    }
}
