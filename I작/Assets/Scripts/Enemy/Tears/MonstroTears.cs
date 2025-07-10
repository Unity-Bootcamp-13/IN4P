using UnityEngine;



public class MonstroTears : MonoBehaviour
{
    private float speed;
    private float range;
    private float damage;
    private Vector3 target;
    private float timer = 0;
    private float lifetime = 3f;
    private float damageRange = 0.1f;
    private bool _hasArrived = false;
    private bool launched;
    public bool HasArrived => _hasArrived;

    Animator tearsAnimator;
    Rigidbody2D rb;
    private void Awake()
    {
        tearsAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }



    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            ExplodeAndDestroyTears();
        }

        if (launched || !_hasArrived)
        {
            float dist = Vector2.Distance(transform.position, target);

            if (dist <= damageRange)
            {
                _hasArrived = true;
                ExplodeAndDestroyTears();
            }
        }

    }

    void ExplodeAndDestroyTears()
    {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        tearsAnimator.SetTrigger("Pop");

        Destroy(gameObject, 0.2f);
        enabled = false;
    }

    public void SetTears(float Enemy_speed, float Enemy_range, float Enemy_damage)
    {
        speed = Enemy_speed;
        range = Enemy_range;
        damage = Enemy_damage;
    }

    public void LaunchTo(Vector3 targetPos, float _speed)
    {
        target = targetPos;
        speed = _speed;
        // ���� ���� ���
        Vector2 start2D = transform.position;
        Vector2 end2D = targetPos;
        float dx = Mathf.Abs(end2D.x - start2D.x);
        float dy = end2D.y - start2D.y;
        float vx = speed;  // ����ӵ� ũ��
        float vy;

        // �߷°� (Physics2D.gravity.y�� ���� -9.81)
        float g = Physics2D.gravity.y * rb.gravityScale;  // ����

        // �����̵� �Ǻ�
        if (dx < 0.001f)
        {
            //dy�� ����϶� �����߻� �ƴҰ�� 0
            vy = dy > 0
                  ? Mathf.Sqrt(2f * -g * dy)
                  : 0f;
        }
        else
        {
            // ���� �ð� t = dx / horizontalSpeed
            float t = dx / vx;
            //�ʱ� �����ӵ� vy = (dy - 0.5 * g * t^2) / t
            vy = (dy - 0.5f * g * t * t) / t;
        }

        // ���� ���� ����
        Vector2 dir = (end2D - start2D).normalized;
        Vector2 velocity = new Vector2(dir.x * vx, vy);

        // Rigidbody�� ����
        rb.linearVelocity = velocity;
    }

    public void OnHitFrame()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            damageRange
            );
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var player = hit.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage((int)damage);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            Destroy(gameObject);
        }
    }
}
