using System.ComponentModel;
using UnityEngine;

public class PlayerHeadMovement : MonoBehaviour
{

    public PlayerStat stat;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CheckHeadKey(string direction)
    {
        animator.SetBool("Head_Left", false);
        animator.SetBool("Head_Right", false);
        animator.SetBool("Head_Down", false);
        animator.SetBool("Head_Up", false);

        animator.SetBool(direction, true);
    }

    public void SetHeadAnimation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CheckHeadKey("Head_Left");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            CheckHeadKey("Head_Right");
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            CheckHeadKey("Head_Up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            CheckHeadKey("Head_Down");
        }
        

        else
        {
            animator.SetBool("Head_Left", false);
            animator.SetBool("Head_Right", false);
            animator.SetBool("Head_Down", false);
            animator.SetBool("Head_Up", false);

        }
    }
}
