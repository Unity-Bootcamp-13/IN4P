using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
public interface IDamageable
{
    void TakeDamage(int amount, Vector2? dir);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public RoomController roomcontroller;
    protected Player target;
    [SerializeField]protected CharacterData EnemyData;
    protected int enemy_hp;
    protected float enemy_atk;
    protected float enemy_atkSpeed;
    protected float enemy_speed;
    protected float enemy_atkRange;
    protected float enemy_projectileSpeed;
    protected readonly float damageInterval = 1f;
    protected readonly int enemy_OneatkDamage = 2;

    public System.Action OnDeath;

    protected Animator enemy_Animator;
    protected Rigidbody2D enemy_rb;
    protected Coroutine _damageRoutine;
    protected Collider2D enemy_Collider;

    protected virtual void Awake()
    {
        enemy_hp = EnemyData.PlayerHp;
        enemy_atk = EnemyData.Atk;
        enemy_atkSpeed = EnemyData.AtkSpeed;
        enemy_speed = EnemyData.Speed;
        enemy_atkRange = EnemyData.AtkRange;
        enemy_projectileSpeed = EnemyData.ProjectileSpeed;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy_Animator = GetComponent<Animator>();
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_Collider = GetComponent<Collider2D>();
    }

    protected virtual async void Start()
    {
        await Task.Delay(500);
    }
    public virtual void TakeDamage(int amount, Vector2? attackOrigin)
    {
        enemy_hp -= amount;
        if (enemy_hp <= 0)
        {
            Die();
        }

    }


    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _damageRoutine == null)
        {
            _damageRoutine = StartCoroutine(DamageLoop());
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _damageRoutine != null)
        {
            StopCoroutine(_damageRoutine);
            _damageRoutine = null;
        }
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            if (enemy_atk == 0)
            {
                break;
            }
            target.TakeDamage((int)enemy_atk);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        enemy_Collider.enabled = false;
        Destroy(gameObject);
    }
}