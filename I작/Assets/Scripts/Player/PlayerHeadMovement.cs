using System.ComponentModel;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        stat = GetComponentInParent<PlayerStat>();
        delay = 1f / stat.atkspeed;
    }

    public void CheckHeadKey(string direction)
    {

        animator.SetBool("Head_Left", false);
        animator.SetBool("Head_Right", false);
        animator.SetBool("Head_Down", false);
        animator.SetBool("Head_Up", false);
        isAttackDelay = true;
        animator.SetBool(direction, true);
       
    }

    public void SetHeadAnimation()
    {
        currentTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CheckHeadKey("Head_Left");

            if (delay <= currentTime)
            {
                currentTime = 0;
                animator.SetBool("IsAttack", true);
                GameObject go = Instantiate(tearsPrefab);
                Vector3 offset = new Vector3(-0.5f, 0, 0);
                go.transform.position = transform.position + offset;
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            CheckHeadKey("Head_Right");
            if (delay <= currentTime)
            {
                currentTime = 0;
                animator.SetBool("IsAttack", true);
                GameObject go = Instantiate(tearsPrefab);
                Vector3 offset = new Vector3(0.5f, 0, 0);
                go.transform.position = transform.position + offset;
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            CheckHeadKey("Head_Up");

            if (delay <= currentTime)
            {
                currentTime = 0;
                animator.SetBool("IsAttack", true);
                GameObject go = Instantiate(tearsPrefab);
                Vector3 offset = new Vector3(0, 0.5f, 0);
                go.transform.position = transform.position + offset;
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            CheckHeadKey("Head_Down");

            if (delay <= currentTime)
            {
                currentTime = 0;
                animator.SetBool("IsAttack", true);
                GameObject go = Instantiate(tearsPrefab);
                Vector3 offset = new Vector3(0, -0.5f, 0);
                go.transform.position = transform.position + offset;
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
}
