using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemytears : MonoBehaviour
{
    public float speed;
    public float range;
    public Vector3 target;
    public float timer;
    bool lunched;
    public Animator tearsAnimator;
    Rigidbody2D rb;
    private void Awake()
    {
        tearsAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            DestoryTears();
        }
        
        if (!lunched) return;

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            DestoryTears();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag �����ؼ� ���͸� ������ �ɷ� ����
        if (collision.gameObject.tag == "Player")
            DestoryTears();
          
    }

    void DestoryTears()
    {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        tearsAnimator.SetBool("Pop", true);
       
        Destroy(gameObject,0.2f);
        enabled = false;
    }

    public void LaunchTo(Vector3 targetPos, float _speed)
    {
        target = targetPos;
        speed = _speed;
        lunched = true;
        // ���� ���� ���
        Vector2 start2D = transform.position;
        Vector2 end2D = targetPos;
        float dx = Vector2.Distance(
            new Vector2(end2D.x, 0),
            new Vector2(start2D.x, 0)
        );
        float dy = end2D.y - start2D.y;

        // 2) �߷°� (Physics2D.gravity.y�� ���� -9.81)
        float g = Physics2D.gravity.y * rb.gravityScale;  // ����

        // 3) ���� �ð� t = dx / horizontalSpeed
        float vx = speed; // ���� �ӵ� ũ��
        float t = dx / vx;

        // 4) �ʱ� �����ӵ� vy = (dy - 0.5 * g * t^2) / t
        float vy = (dy - 0.5f * g * t * t) / t;

        // 5) ���� ���� ����
        Vector2 dir = (end2D - start2D).normalized;
        Vector2 velocity = new Vector2(dir.x * vx, vy);

        // 6) Rigidbody�� ����
        rb.linearVelocity = velocity;
        // gravityScale�� �̹� Awake���� ���õ� �ִٰ� ����
    }
}

