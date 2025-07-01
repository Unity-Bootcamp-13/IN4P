using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEngine;

public enum BossState
{
    Idle,
    LowJump,
    HighJump,
    BloodAttack,
}

public class Monstro : MonoBehaviour
{
    public int cooltime = 2;
    public string boss_name = "Monstro";
    public int boss_hp = 250;
    public float boss_speed = 2;
    public float boss_knockback;
    public int boss_oneDamage = 2;
    public int boss_halfDamage = 1;
    public float boss_attackSpeed = 5f;
    public float boss_MoveRange = 5f;
    public float jump_height = 2.5f;
    public float jumpDuration = 0.5f;

    private BossState _bossState = BossState.Idle;

    public Transform player;
    public Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Coroutine _patternRoutine;
    private Collider2D collider;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(250); 
        }
        float facing = spriteRenderer.flipX ? 1f : -1f;
        if (_bossState == BossState.Idle && player != null && _patternRoutine == null)
        {
            _patternRoutine = StartCoroutine(LowJumpWithCooldown());
        }

    }

    private IEnumerator LowJumpWithCooldown()
    {
        _bossState = BossState.LowJump;

        yield return StartCoroutine(LowJumpRoutine());

        yield return new WaitForSeconds(cooltime);

        _bossState = BossState.Idle;
        _patternRoutine = null;
    }

    //�����ϸ鼭 ĳ���� ������ �ٰ���
    private IEnumerator LowJumpRoutine()
    {
        // �ִϸ����� Ʈ���� (�ִϸ��̼� ������ �ʿ��ϴٸ�)
        animator.SetTrigger("LowJump");

        Vector3 playerPos = player.position;
        Vector3 startPos = transform.position;


        Vector3 endPos;
        // �÷��̾�� ĳ���Ͱ��� �Ÿ��� ����ؼ� boss_MoveRange���� ������ �÷��̾���ġ��
        float currentDist = Vector3.Distance(startPos, playerPos);

        if (boss_MoveRange >= currentDist)
        {
            endPos = new Vector3(playerPos.x, playerPos.y, 0);
        }
        else
        {
            Vector3 dir = (playerPos - startPos).normalized;
            endPos = startPos + dir * boss_MoveRange;
        }
        float elapsed = 0f;
       
        //  �� ����Ŭ(��¡��ϰ�) ���� ����
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);

            // (a) ���� ����
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            // (b) ���� ������ (������)
            pos.y = Mathf.Lerp(startPos.y, endPos.y, t)
                  + jump_height * 4f * t * (1f - t);

            transform.position = pos;
            yield return null;
        }
       
        transform.position = endPos;

    }


    //�� ���ϱ�
    public void BloodAttack()
    {

    }

    public void TakeDamage(int damage)
    {
        boss_hp -= damage;
        if (boss_hp <= 0)
        {
            bossDie();
        }
    }

    //���� ���
    public void bossDie()
    {
        collider.enabled = false;
        enabled = false;  // Update ��Ȱ��ȭ ��

        // �״� �ִϸ��̼� Ʈ����
        animator.SetTrigger("Dead");

        StartCoroutine(DeathShake());
    }

    private IEnumerator DeathShake()
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.1f;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float xOffset = UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);
            yield return null;
        }
        transform.position = originalPosition;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}


