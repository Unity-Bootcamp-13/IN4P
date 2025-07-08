using UnityEngine;

public class BrimstoneBeam : MonoBehaviour
{
    private Vector2 direction;
    private float duration = 1.2f;
    private float damageInterval = 1.2f/9;

    private float damage;
    private float timer;
    private float damageTimer;

    private LineRenderer lineRenderer;
    private float beamLength = 20f;

    private Transform firePoint;

    public void Initialize(Vector2 dir, float damage, Transform mouse)
    {
        this.direction = dir.normalized;
        this.damage = damage;
        this.firePoint = mouse;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        timer = duration;
        damageTimer = 0f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;

        if (timer <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateBeam();

        if (damageTimer <= 0f)
        {
            damageTimer = damageInterval;
            DealDamage();
        }
    }

    private void UpdateBeam()
    {
        if (firePoint == null)
            return;

        Vector3 start = firePoint.position;
        Vector3 end = start + (Vector3)direction * beamLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void DealDamage()
    {
        if (firePoint == null)
            return;

        Vector2 start = firePoint.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, beamLength);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Monster") || hit.collider.tag == "Boss")
            {
                Enemy monster = hit.collider.GetComponent<Enemy>();
                if (monster != null)
                {
                    monster.TakeDamage((int)damage,null);
                }
            }
        }
    }
}
