using System.ComponentModel;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class PlayerHeadMovement : MonoBehaviour
{
    private PlayerStat stat;
    private Animator animator;
    [SerializeField] GameObject tearsPrefab;

    bool isAttackDelay;
    public float delay;
    public float currentTime = 0;


    Vector3 RightDirection = new Vector3(0.5f, 0, 0);
    Vector3 LeftDirection = new Vector3(-0.5f, 0, 0);
    Vector3 UpDirection = new Vector3(0, 0.5f, 0);
    Vector3 DownDirection = new Vector3(0, -0.5f, 0);

    void Start()
    {
        animator = GetComponent<Animator>();
        stat = GetComponentInParent<PlayerStat>();

        delay = 2 / stat.atkspeed;
    }


    public void SetHeadAnimation()
    {
        currentTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("Head_Left", true);
            if (delay <= currentTime)
            {
                CreateTears(LeftDirection);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("Head_Right", true);

            if (delay <= currentTime)
            {
                CreateTears(RightDirection);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Head_Up", true);


            if (delay <= currentTime)
            {
                CreateTears(UpDirection);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("Head_Down", true);


            if (delay <= currentTime)
            {
                CreateTears(DownDirection);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
        else
        {
            animator.SetBool("Head_Left", false);
            animator.SetBool("Head_Right", false);
            animator.SetBool("Head_Down", false);
            animator.SetBool("Head_Up", false);
           
        }

    }


    public void CreateTears(Vector3 dir)
    {
        currentTime = 0;
        animator.SetBool("IsAttack", true);
        GameObject go = Instantiate(tearsPrefab);
        go.transform.position = transform.position + dir;
    }
}
