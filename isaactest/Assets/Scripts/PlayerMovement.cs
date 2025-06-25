using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private Vector2 last = Vector2.down;


    public void Update()
    {
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector2(h, v);
        SetBodyAnimation(dir);
        transform.position += dir * stat.speed * Time.deltaTime;
        SetHeadAnimation();
    }

    private void SetHeadAnimation()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetTrigger("Head_Left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetTrigger("Head_Right");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetTrigger("Head_Up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetTrigger("Head_Down");
        }
    }

    private void SetBodyAnimation(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("IsMove", true);
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                last = direction.normalized;
            }
            else
            {
                animator.SetBool("IsMove",false);
                animator.SetFloat("Horizontal", last.x);
                animator.SetFloat("Vertical", last.y);

            }
        }
    }
}
