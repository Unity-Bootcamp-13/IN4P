using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Collider2D portalCollider;
    public Transform targetPosition;
    public Action<int> portalAction;

        
    public int thisDirction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = targetPosition.position;
            portalAction.Invoke(thisDirction);

        }
    }

    public void CloseDoor()
    {
        // TODO 애미네이션 추가

        portalCollider.enabled = false;
    }

    public void OpenDoor()
    {
        portalCollider.enabled = true;
    }
}