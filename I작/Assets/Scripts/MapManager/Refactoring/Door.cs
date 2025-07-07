using System;
using UnityEngine;

public enum DoorType
{
    Normal,
    Boss,
    Treasure,
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
            collider.transform.position = targetPosition.position;
            portalAction.Invoke(thisDirction);
        }
    }

    public void OpenDoor()
    {
        if (type != DoorType.Treasure)
            transform.GetChild(0).gameObject.SetActive(false);

        portalCollider.enabled = true;
    }

    public void CloseDoor()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        portalCollider.enabled = false;
    }
}