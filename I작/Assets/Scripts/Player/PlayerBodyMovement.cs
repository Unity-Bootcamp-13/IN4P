using UnityEngine;

public class PlayerBodyMovement : MonoBehaviour
{
    public PlayerStat stat;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CheckBodyKey(string direction)
    {
        animator.SetBool("Body_Left", false);
        animator.SetBool("Body_Right", false);
        animator.SetBool("Body_Down", false);
        animator.SetBool("Body_Up", false);

        animator.SetBool(direction, true);
    }

    

    public void SetBodyAnimation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            CheckBodyKey("Body_Left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            CheckBodyKey("Body_Right");
        }
        else if (Input.GetKey(KeyCode.W))
        {
           CheckBodyKey("Body_Up");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            CheckBodyKey("Body_Down");
        }


        else
        {
            animator.SetBool("Body_Left", false);
            animator.SetBool("Body_Right", false);
            animator.SetBool("Body_Down", false);
            animator.SetBool("Body_Up", false);

        }
    }


}
