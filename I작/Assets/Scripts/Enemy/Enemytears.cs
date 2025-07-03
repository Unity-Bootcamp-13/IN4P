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

        
        // 1) ��ǥ �������� ���� �Ÿ�
        float dist = Vector2.Distance(transform.position, target);

        // 2) �Ÿ� ���ϰ� �Ǹ� �����ޡ� ó��
        if (dist <= damageRange)
        {
            _hasArrived = true;            // �ڷ�ƾ ��� ������ �÷���
            HitPlayer();                   // ������ ������
            ExplodeAndDestroyTears();           // �� �ִϸ��̼� & ����
        }
    }

    private void HitPlayer()
    {
        // �ܼ��� Tag �˻� �� ������ ȣ�� ����
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
                    Debug.Log("�÷��̾� ������ 2 ����");
                    //player.TakeDamage(damage);
                    break;
                }
            }
        }
    }
}

