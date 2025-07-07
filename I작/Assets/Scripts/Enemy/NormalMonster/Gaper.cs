using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Gaper : Enemy
{
    [SerializeField] CharacterData GaperData;
    public float enemy_atk;
    public float enemy_atkSpeed;
    public float enemy_speed;
    public float enemy_atkRange;
    public float enemy_projectileSpeed;
    private readonly float damageInterval = 1f;
    public Animator enemy_Animator;

    public Player target;
    public Rigidbody2D enemy_rb;
    public Collider2D enemy_Collider;
    private Coroutine _damageRoutine;
    public GameObject GusherPrefab;

    private void Awake()
    {
        hp = GaperData.PlayerHp;
        enemy_atk = GaperData.Atk;
        enemy_atkSpeed = GaperData.AtkSpeed;
        enemy_speed = GaperData.Speed;
        enemy_atkRange = GaperData.AtkRange;

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy_Animator = transform.GetChild(0).GetComponent<Animator>();
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_Collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        ChasePlayerMovement();
    }


    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
    }
    public void ChasePlayerMovement()
    {
        Vector2 direciton = (target.transform.position - transform.position).normalized;
        enemy_rb.MovePosition(enemy_rb.position + direciton * enemy_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && _damageRoutine == null)
        {
            _damageRoutine = StartCoroutine(DamageLoop());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player") && _damageRoutine != null)
        {
            StopCoroutine(_damageRoutine);
            _damageRoutine = null;
        }
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            target.TakeDamage((int)enemy_atk);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public override void Die()
    {
        enemy_Collider.enabled = false;

        StartCoroutine(Died());
    }

    private IEnumerator Died()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand > 0)
        {
            StartCoroutine(SpawnGusher());
        }
        this.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator SpawnGusher()
    {
        Instantiate(GusherPrefab, transform.position, quaternion.identity);
        yield return null;
    }
}
