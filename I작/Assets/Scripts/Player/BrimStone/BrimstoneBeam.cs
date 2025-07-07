using UnityEngine;

public class BrimstoneBeam : MonoBehaviour
{
    public Vector2 direction;
    public float duration = 1f;
    public float damageInterval = 0.083f;

    private float damage;
    private float timer;
    private float damageTimer;

    private LineRenderer lineRenderer;
    private float beamLength = 10f;

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
            if (hit.collider.CompareTag("Monster"))
            {
                ContactMonster monster = hit.collider.GetComponent<ContactMonster>();
                if (monster != null)
                {
                    monster.TakeDamage((int)damage);
                }
            }
        }
    }
}
