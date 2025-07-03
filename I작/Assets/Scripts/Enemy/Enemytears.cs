using Unity.VisualScripting;
using UnityEditor.Rendering.Analytics;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public class Enemytears : MonoBehaviour
{
    public float speed;
    public float range;
    public Vector3 target;
    public float timer=0;
    public float lifetime = 5f;
    public float damageRange = 0.1f;
    public int damageAmount = 1;
    bool launched;
    private bool _hasArrived = false;
    public bool HasArrived => _hasArrived;

    Collider2D tearscollider;
    Animator tearsAnimator;
    Rigidbody2D rb;
    private void Awake()
    {
        tearsAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tearscollider = GetComponent<Collider2D>();
    }

    
    
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > lifetime)
        {
            ExplodeAndDestroyTears();
        }
        if (!launched || _hasArrived) return;

        
        // 1) 목표 지점까지 남은 거리
        float dist = Vector2.Distance(transform.position, target);

        // 2) 거리 이하가 되면 “도달” 처리
        if (dist <= damageRange)
        {
            _hasArrived = true;            // 코루틴 대기 해제용 플래그
            HitPlayer();                   // 데미지 입히기
            ExplodeAndDestroyTears();           // 팝 애니메이션 & 삭제
        }
    }

    private void HitPlayer()
    {
        // 단순히 Tag 검색 후 데미지 호출 예시
        Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRange,
                            LayerMask.GetMask("Player"));
        if (hit != null)
        {
            var player = hit.GetComponent<Player>();
        }
    }

    void ExplodeAndDestroyTears()
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
        launched = true;
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

    public void OnHitFrame(int Damage)
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
                    Debug.Log("플레이어 데미지 2 받음");
                    //player.TakeDamage(damage);
                    break;
                }
            }
        }
    }
}

