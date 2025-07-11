using UnityEngine;

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

    public void OpenSecretDoor()
    {
        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        currentColor.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;

        secretDoor.portalCollider.enabled = true;
        secretDoor.portalCollider.isTrigger = true;
    }
}
