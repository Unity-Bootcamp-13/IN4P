using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
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

            yield return StartCoroutine(SpawnGusher());
        }
        
        // this.gameObject.SetActive(false);

        base.Die();

    }

    private IEnumerator SpawnGusher()
    {
        
        GameObject gusher = Instantiate(GusherPrefab, transform.position, Quaternion.identity);
        var controller = roomcontroller as NormalRoomController;
        if (controller == null)
        {
            Debug.LogError($"[{name}] roomcontroller가 할당되지 않았거나 NormalRoomController가 아닙니다.");
            yield break;
        }
        controller.monsterCount++;
        Enemy gusherEnemy = gusher.GetComponentInChildren<Enemy>();
        if (gusherEnemy != null)
        {
            gusherEnemy.roomcontroller = controller;
            gusherEnemy.OnDeath += controller.CheckClearCondition;
            Debug.Log("Gusher에 OnDeath 구독 완료.");
        }
        Debug.Log($"[SpawnGusher] monsterCount 증가. 현재: {controller.monsterCount}");
        yield return null;
        // this.gameObject.SetActive(false);
        //base.Die();

    }
}
