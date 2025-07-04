using System.Collections;
using UnityEngine;

public class ContactMonster : MonoBehaviour
{
    [SerializeField] CharacterData monsterData;
    public int enemy_hp;
    public float enemy_atk;
    public float enemy_atkSpeed;
    public float enemy_speed;
    public float enemy_atkRange;
    public float enemy_projectileSpeed;
    public int enemy_currentHp;

    public Animator enemyAnimator;

    public GameObject target;
    public Rigidbody2D enemy_rb;

    private void Awake()
    {
        enemy_hp = monsterData.PlayerHp;
        enemy_atk = monsterData.Atk;
        enemy_atkSpeed = monsterData.AtkSpeed;
        enemy_speed = monsterData.Speed;
        enemy_atkRange = monsterData.AtkRange;
        enemy_currentHp = enemy_hp;

        target = GameObject.FindGameObjectWithTag("Player");
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

    public void TakeDamage(int damage)
    {
        enemy_hp -= damage;

        if (enemy_hp <= 0)
        {
            enemyAnimator.SetTrigger("IsDead");
            StartCoroutine(DieAnimation());
        }
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
