using UnityEngine;

public class BrimStoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.Instance.PlaySFX(SFX.PassiveItem);
            collision.transform.GetChild(0).gameObject.GetComponent<Attack>().SwitchToBrimstone();
            collision.GetComponent<Player>().AquireItemAnim(GetComponent<SpriteRenderer>().sprite);
            Destroy(gameObject);
        }        
    }
}
