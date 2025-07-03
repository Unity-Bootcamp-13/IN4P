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

    public Attack attack;

    public Transform target;
    public Rigidbody2D enemy_rb;

    public Vector2 destination;

    private Player player;
    private Tears tears;

    private void Awake()
    {
        enemy_hp = monsterData.PlayerHp;
        enemy_atk = monsterData.Atk;
        enemy_atkSpeed = monsterData.AtkSpeed;
        enemy_speed = monsterData.Speed;
        enemy_atkRange = monsterData.AtkRange;
        enemy_projectileSpeed = monsterData.ProjectileSpeed;
        enemy_currentHp = enemy_hp;

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

    public void RandomMovement()
    {
        destination = new Vector2();
        float x = Random.Range(-9f, 9f);
        float y = Random.Range(-3f, 3f);
        Vector2 targetposition = (Vector2)new Vector3(x, y);

        while (true)
        {
            //var dir = (targetposition - this.transform.position).normalized;
        }
    }

    public void ChasePlayerMovement()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        if (distance > enemy_atkRange)
        {
            Vector2 direciton = (target.position - transform.position).normalized;
            enemy_rb.MovePosition(enemy_rb.position + direciton * enemy_speed * Time.deltaTime);
            enemyAnimator.SetBool("IsAttack", false);
        }
        else
        {
            Debug.Log("АјАн");
            enemyAnimator.SetBool("IsAttack", true);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tears"))
        {
            Tears tears = other.GetComponent<Tears>();

            if (tears != null)
            {
                TakeDamage(tears.damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        enemy_hp -= damage;

        if (enemy_hp <= 0)
        {
            enemyAnimator.SetBool("IsDead", true);
            StartCoroutine(DieAnimation());
        }
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    public void ProjecttileDamage(float damage)
    {

    }
}
