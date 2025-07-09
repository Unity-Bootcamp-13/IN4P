using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAniamtions : MonoBehaviour
{
    public GameObject headObject;
    public GameObject bodyObject;
    public GameObject totalbodyObject;

    public Animator headAnimator;
    public Animator bodyAnimator;
    public Animator totalbodyAnimator;


    public Player player;

    private void Awake()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    // Hurt Animation
    public void AllSpriteOff()
    {
        headObject.GetComponent<SpriteRenderer>().enabled = false;
        bodyObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SwitchBodyHeadAnim()
    {
        headObject.GetComponent<SpriteRenderer>().enabled = true;
        bodyObject.GetComponent<SpriteRenderer>().enabled = true;
        headAnimator.speed = 0f;
        bodyAnimator.speed = 0f;
    }

    public void RestoreAnimator()
    {
        player.HurtAnimFinish();
        player.AquireItemFinish();
        totalbodyObject.SetActive(false);
        headObject.GetComponent<SpriteRenderer>().enabled = true;
        bodyObject.GetComponent<SpriteRenderer>().enabled = true;
        headAnimator.speed = 1f;
        bodyAnimator.speed = 1f;
    }

    // 아이템 획득
    public void AquireItemStart()
    {
        headAnimator.speed = 0f;
        bodyAnimator.speed = 0f;
        totalbodyObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
