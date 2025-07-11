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
        // 방향 벡터 계산
        Vector2 start2D = transform.position;
        Vector2 end2D = targetPos;
        float dx = Mathf.Abs(end2D.x - start2D.x);
        float dy = end2D.y - start2D.y;
        float vx = speed;  // 수평속도 크기
        float vy;

        // 중력값 (Physics2D.gravity.y는 보통 -9.81)
        float g = Physics2D.gravity.y * rb.gravityScale;  // 음수

        // 수평이동 판별
        if (dx < 0.001f)
        {
            //dy가 양수일때 수직발사 아닐경우 0
            vy = dy > 0
                  ? Mathf.Sqrt(2f * -g * dy)
                  : 0f;
        }
        else
        {
            // 비행 시간 t = dx / horizontalSpeed
            float t = dx / vx;
            //초기 수직속도 vy = (dy - 0.5 * g * t^2) / t
            vy = (dy - 0.5f * g * t * t) / t;
        }

        // 방향 단위 벡터
        Vector2 dir = (end2D - start2D).normalized;
        Vector2 velocity = new Vector2(dir.x * vx, vy);

        // Rigidbody에 적용
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
