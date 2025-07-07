using UnityEngine;

public class DroppedBomb : MonoBehaviour
{
    public Animator bombAnimator;
    int acquire;


    private void Start()
    {
         acquire = Animator.StringToHash("Acquire");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().bombCount++;
            bombAnimator.SetTrigger(acquire);
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}