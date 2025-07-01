using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public Animator chestAnimator;

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if(player.keyCount > 0 )
            {
                player.keyCount--;
                OpenTreasureChest();
            }
        }
    }

    private void OpenTreasureChest()
    {
        Debug.Log("보물 등장");
        chestAnimator.SetTrigger("OpenChest");
    }

    void DestroyChest()
    {
        Destroy(gameObject);
    }
}
