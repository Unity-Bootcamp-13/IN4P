using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;

public enum BossPatturn
{
    Idle,
    LowJump,
    HighJump,
    BloodAttack,
}

public enum BossState
{
    Ground,
    jump
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
    public float JumpDuration = 0.5f;

    private BossPatturn _bossPatturn = BossPatturn.Idle;
    private BossState _bossState = BossState.Ground;

    private GameObject player;
    private Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Coroutine _patternRoutine;
    private Collider2D collider;
    public GameObject tears;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        collider.enabled = (_bossState == BossState.Ground);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(250);
        }

        if (_bossPatturn == BossPatturn.Idle && _patternRoutine == null)
        {
            int pattern = UnityEngine.Random.Range(0, 2);
            _patternRoutine = StartCoroutine(patternWithCooldown(pattern));
        }

    }

    private IEnumerator patternWithCooldown(int pattern)
    {
        _bossState = BossState.jump;
        float facing = spriteRenderer.flipX ? 1f : -1f;
        Vector3 dir = player.transform.position - transform.position;
        spriteRenderer.flipX = dir.x > 0f;

        // switch (pattern)
        // {
        //     case 0: // ���� ����
        //         _bossState = BossState.LowJump;
        //         yield return StartCoroutine(LowJumpRoutine());
        //         break;
        //     case 1: // �� ���ϱ�
        //         _bossState = BossState.BloodAttack;
        //         yield return StartCoroutine(BloodAttackRoutine());
        //         break;
        //     case 2: // ���� ���� 
        //         _bossState = BossState.HighJump;
        //         yield return StartCoroutine(HighJumpRoutine());
        //         break;
        //     
        // }
        _bossPatturn = BossPatturn.BloodAttack;
        yield return StartCoroutine(BloodAttackRoutine());

        yield return new WaitForSeconds(cooltime);
        _bossPatturn = BossPatturn.Idle;
        _bossState = BossState.Ground;
        _patternRoutine = null;
    }

    //�����ϸ鼭 ĳ���� ������ �ٰ���
    private IEnumerator LowJumpRoutine()
    {

        animator.SetTrigger("LowJump");

        Vector3 playerPos = player.transform.position;
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
        while (elapsed < JumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);

            // ���� ����
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            // ���� ������ (������)
            pos.y = Mathf.Lerp(startPos.y, endPos.y, t)
                  + jump_height * 4f * t * (1f - t);

            transform.position = pos;
            yield return null;
        }

        transform.position = endPos;

    }


    //�� ���ϱ�
    private IEnumerator BloodAttackRoutine()
    {
        animator.SetTrigger("BloodAttack");
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length / stateInfo.speedMultiplier;
        yield return new WaitForSeconds(clipLength);
    }

    private IEnumerator HighJumpRoutine()
    {
        animator.SetTrigger("HighJump");
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length / stateInfo.speedMultiplier;
        yield return new WaitForSeconds(clipLength);
        
    }
    
    public void TakeDamage(int damage)
    {
        boss_hp -= damage;
        if (boss_hp <= 0)
        {
            bossDie();
        }
    }

    
    public void spriteRendereroff()
    {
        spriteRenderer.enabled = false;
        _bossState = BossState.jump;
    }

    public void spriteRendereron()
    {
        spriteRenderer.enabled = true;
        _bossState = BossState.Ground;
    }

    float apexHeight;             // ����� ����
    float elapsed;                // �ð� üũ

    Vector3 startPos;
    Vector3 playerPos;
    Vector3 landPos;

    public void LongJump()
    {
        StartCoroutine(LongJumpRoutine());
    }

    private IEnumerator LongJumpRoutine()
    {
        apexHeight = jump_height * 4f;
        startPos = transform.position;
        
        float elapsed = 0f;
    
        while (elapsed < JumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);

            // ���� ������ �״��, ������ apexHeight �߰�
            Vector3 pos = Vector3.Lerp(startPos, startPos, t);
            pos.y = Mathf.Lerp(startPos.y, startPos.y, t) + apexHeight * 4f * t * (1f - t);
            transform.position = pos;
            yield return null;
        }
    }

    public void playerSearch()
    {
        StartCoroutine(playerSearchRoutine());
    }
    private IEnumerator playerSearchRoutine()
    {
        Debug.Log("�÷��̾� Ž������");
        playerPos = player.transform.position;
        startPos = transform.position;

        float elapsed = 0f;
        float Duration = JumpDuration;

        float apexY = startPos.y;
        float x = 0;
        float targetX = playerPos.x;
        
        landPos = player.transform.position;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            x = Mathf.Lerp(startPos.x, targetX, t);
            transform.position =new  Vector3(x,apexY,t);
            yield return null;
        }
        
        Debug.Log("�÷��̾� Ž�� ��");
    }
    public void Fall()
    {
        StartCoroutine(FallRoutine());
    }
    private IEnumerator FallRoutine()
    {
        elapsed = 0f;
        Vector3 fallStart = transform.position;
        while (elapsed < JumpDuration/5)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);
            transform.position = Vector3.Lerp(fallStart, landPos, t);
            yield return null;
        }
        transform.position = landPos;
    }
    //���� �߻� �ڷ�ƾ
    public void BloodShot()
    {
        StartCoroutine(BloodShotRoutine());
    }

    private IEnumerator BloodShotRoutine()
    {
        Vector3 headPos = transform.position + Vector3.up * 1.2f;
        Vector3 playerPos = player.transform.position;

        int tearCount = 12;
        float radius = 1.8f;
        float circleStep = 360f / tearCount;

        // 1) �� �����ӿ� 12�� ���ÿ� ����
        for (int i = 0; i < tearCount; i++)
        {
            // 2) �÷��̾� �ֺ� ��ǥ ����
            float angle = circleStep * i;
            Vector2 offset = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * radius;
            Vector3 targetPos = playerPos + (Vector3)offset;

            // 3) ���� �� Launch ȣ��
            GameObject t = Instantiate(tears, headPos, Quaternion.identity);
            if (t.TryGetComponent<Enemytears>(out var et))
            {
                et.LaunchTo(targetPos, boss_attackSpeed);
            }
        }

        yield break;
    }

    public void bossDie()
    {
        collider.enabled = false;
        enabled = false;  

        animator.SetTrigger("Dead");

        StartCoroutine(DeathShake());
    }

    private IEnumerator DeathShake()
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 1.5f;
        float shakeMagnitude = 0.2f;
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


