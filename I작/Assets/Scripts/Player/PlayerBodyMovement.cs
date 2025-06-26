using UnityEngine;

public class PlayerBodyMovement : MonoBehaviour
{
    public PlayerStat stat;
    public Rigidbody2D rid;
    private Animator animator;

    private Vector2 moveInput;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rid = GetComponentInParent<Rigidbody2D>();
    }



    public void SetBodyAnimation()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        

        if (h < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Body_Left", true);
        }
        else if (h > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("Body_Right", true);
        }
        else if (v < 0)
        {
            animator.SetBool("Body_Up", true);
        }
        else if (v > 0)
        {
            animator.SetBool("Body_Down", true);
        }
        else
        {
            animator.SetBool("Body_Left", false);
            animator.SetBool("Body_Right", false);
            animator.SetBool("Body_Up", false);
            animator.SetBool("Body_Down", false);

        }

        Vector3 dir = new Vector2(h, v).normalized;
        rid.linearVelocity = dir * stat.speed;
    }


}
