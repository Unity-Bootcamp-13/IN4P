using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SecretDoorController : MonoBehaviour
{
    public Door secretDoor;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OpenSecretDoor();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        secretDoor.OpenDoor();
    //        transform.GetChild(0).gameObject.SetActive(false);
    //        //secretDoor.portalCollider.isTrigger = true;
    //
    //        Vector3 pushDirection = collision.contacts[0].normal;
    //
    //        collision.gameObject.transform.position -= pushDirection * 0.5f;
    //
    //    }
    //}


    public void OpenSecretDoor()
    {
        Debug.Log("ºñ»ó");
        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        currentColor.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;

        secretDoor.portalCollider.enabled = true;
        secretDoor.portalCollider.isTrigger = true;
    }
}
