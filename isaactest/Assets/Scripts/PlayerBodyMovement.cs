using UnityEngine;

public class PlayerBodyMovement : MonoBehaviour
{
    public PlayerStat stat;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector2(h, v);
        SetBodyAnimation(dir);
        transform.position += dir * stat.speed * Time.deltaTime;
    }


    private Vector2 last = Vector2.down;


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
                animator.SetBool("IsMove", false);
                animator.SetFloat("Horizontal", last.x);
                animator.SetFloat("Vertical", last.y);

            }
        }
    }
}
