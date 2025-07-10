using System.Collections;
using UnityEngine;

public class ContectMonster : Enemy
{
    [SerializeField] CharacterData monsterData;
    public int enemy_hp;
    public float enemy_atk;
    public float enemy_atkSpeed;
    public float enemy_speed;
    public float enemy_atkRange;
    public float enemy_projectileSpeed;
    private readonly float damageInterval = 1f;
    public Animator enemyAnimator;


    private Coroutine _damageRoutine;
    public Player target;
    public Rigidbody2D enemy_rb;


    private void Awake()
    {
        enemy_hp = monsterData.PlayerHp;
        enemy_atk = monsterData.Atk;
        enemy_atkSpeed = monsterData.AtkSpeed;
        enemy_speed = monsterData.Speed;
        enemy_atkRange = monsterData.AtkRange;
        hp = enemy_hp;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyAnimator = GetComponent<Animator>();
        enemy_rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        ChasePlayerMovement();
    }

    public void ChasePlayerMovement()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);

        if (distance > enemy_atkRange)
        {
            Vector2 direciton = (target.transform.position - transform.position).normalized;
            enemy_rb.MovePosition(enemy_rb.position + direciton * enemy_speed * Time.deltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _damageRoutine == null)
        {
            _damageRoutine = StartCoroutine(DamageLoop());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
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
            if(enemy_atk == 0)
            {
                break;
            }
            target.TakeDamage((int)enemy_atk);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public override void Die()
    {
        Debug.Log("ÆÄ¸® Á×À½");
        enemyAnimator.SetTrigger("IsDead");
        StartCoroutine(DieAnimation());
        
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySFX(SFX.Monster_Die);
        base.Die();
    }
}
