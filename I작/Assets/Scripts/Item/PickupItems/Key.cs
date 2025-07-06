using UnityEngine;

public class Key : MonoBehaviour
{
    public Animator keyAanimator;
    int acquire;


    private void Start()
    {
        acquire = Animator.StringToHash("Acquire");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().keyCount++;
            keyAanimator.SetTrigger(acquire);
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
