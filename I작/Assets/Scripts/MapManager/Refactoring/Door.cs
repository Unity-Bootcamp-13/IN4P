using System;
using NUnit.Framework.Internal;
using UnityEngine;

public enum DoorType
{
    Normal,
    Boss,
    Treasure,
    Secret
}

public class Door : MonoBehaviour
{
    public DoorType type;

    public Collider2D portalCollider;
    public Transform targetPosition;

    public int thisDirction;
    public Action<int> portalAction;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Player>().RevertStats();
            collider.transform.position = targetPosition.position;
            portalAction.Invoke(thisDirction);
        }
    }

    public void OpenDoor()
    {
        if (!(type == DoorType.Treasure || type == DoorType.Secret))
            transform.GetChild(0).gameObject.SetActive(false);

        portalCollider.enabled = true;
    }

    public void CloseDoor()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        portalCollider.enabled = false;

        if (type == DoorType.Secret)
        {
            Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
            currentColor.a = 0f;
            gameObject.GetComponent<SpriteRenderer>().color = currentColor;

            portalCollider.isTrigger = false;
        }
    }
}