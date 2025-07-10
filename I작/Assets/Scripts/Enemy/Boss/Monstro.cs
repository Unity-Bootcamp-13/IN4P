using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

public class Monstro : Enemy
{
    public float boss_knockback= 0.7f;
    public float jump_height = 2.5f;
    public float JumpDuration = 0.5f;

    private BossPatturn _bossPatturn = BossPatturn.Idle;
    private BossState _bossState = BossState.Ground;

    protected SpriteRenderer spriteRenderer;
    private Coroutine _patternRoutine;
    public GameObject tears;
    

    private readonly int LowJump = Animator.StringToHash("LowJump");
    private readonly int HighJump = Animator.StringToHash("HighJump");
    private readonly int BloodAttack = Animator.StringToHash("BloodAttack");
    private readonly int Dead = Animator.StringToHash("Dead");

    public static Action<float> onBossHpSlider;
    public static Action onDeath;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        onBossHpSlider?.Invoke(enemy_hp);
    }

    private void Update()
    {
        enemy_Collider.enabled = (_bossState == BossState.Ground);
       
        
        if (_bossPatturn == BossPatturn.Idle && _patternRoutine == null)
        {
            _patternRoutine = StartCoroutine(patternWithCooldown());
        }

    }
    

    private IEnumerator patternWithCooldown()
    {
        float facing = spriteRenderer.flipX ? 1f : -1f;
        Vector3 dir = target.transform.position - transform.position;
        spriteRenderer.flipX = dir.x > 0f;
        int pattern = UnityEngine.Random.Range(0, 3);
        AnimatorStateInfo stateInfo;
        float clipLength;

        switch (pattern)
        {
            case 0: // 낮은 점프
                _bossPatturn = BossPatturn.LowJump;
                yield return StartCoroutine(LowJumpRoutine());
                break;
            case 1: // 피 토하기
                _bossPatturn = BossPatturn.BloodAttack;
                yield return StartCoroutine(BloodAttackRoutine());
                break;
            case 2: // 높은 점프 
                _bossPatturn = BossPatturn.HighJump;
                yield return StartCoroutine(HighJumpRoutine());
                break;
            
        }

        stateInfo = enemy_Animator.GetCurrentAnimatorStateInfo(0);
        clipLength = stateInfo.length / stateInfo.speedMultiplier;
        yield return new WaitForSeconds(clipLength + 1);

        _bossPatturn = BossPatturn.Idle;
        _bossState = BossState.Ground;
        _patternRoutine = null;
    }

    //점프하면서 캐릭터 쪽으로 다가옴
    private IEnumerator LowJumpRoutine()
    {
        enemy_Animator.SetTrigger(LowJump);
        yield return null;
    }

    //피 토하기
    private IEnumerator BloodAttackRoutine()
    {
        enemy_Animator.SetTrigger(BloodAttack);
        yield return null;
    }

    private IEnumerator HighJumpRoutine()
    {
        enemy_Animator.SetTrigger(HighJump);
        yield return null;
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

    float apexHeight;             // 꼭대기 높이
    float elapsed;                // 시간 체크

    Vector3 startPos;
    Vector3 playerPos;
    Vector3 landPos;

    public void Jump()
    {
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        playerPos = target.transform.position;
        startPos = transform.position;


        Vector3 endPos;
        // 플레이어와 캐릭터간의 거리를 계산해서 boss_MoveRange보다 적으면 플레이어위치로
        float currentDist = Vector3.Distance(startPos, playerPos);

        if (enemy_atkRange >= currentDist)
        {
            endPos = new Vector3(playerPos.x, playerPos.y, 0);
        }
        else
        {
            Vector3 dir = (playerPos - startPos).normalized;
            endPos = startPos + dir * enemy_atkRange;
        }
        float elapsed = 0f;

        
        
        while (elapsed < JumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);

            // 수평 보간
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            // 수직 오프셋 (포물선)
            pos.y = Mathf.Lerp(startPos.y, endPos.y, t)
                  + jump_height * 4f * t * (1f - t);

            transform.position = pos;
            yield return null;
        }

        transform.position = endPos;
    }


    public void FildoutJump()
    {
        StartCoroutine(FildoutJumpRoutine());
    }

    private IEnumerator FildoutJumpRoutine()
    {
        apexHeight = jump_height * 2f;
        startPos = transform.position;
        
        float elapsed = 0f;
    
        while (elapsed < JumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);

            // 수평 보간은 그대로, 수직(deltaY)만 추가 
            float deltaY = jump_height * 4f * t * (1f - t);
            Vector3 pos = new Vector3(startPos.x, startPos.y+deltaY, 0);
            transform.position = pos;
            // 올라간 위치 기억
            apexHeight = pos.y;
            yield return null;
        }
        _bossState = BossState.jump;
        transform.position = startPos;
    }

    public void playerSearch()
    {
        StartCoroutine(playerSearchRoutine());
    }
    private IEnumerator playerSearchRoutine()
    {
        playerPos = target.transform.position;
        startPos = transform.position;

        float elapsed = 0f;
        float Duration = JumpDuration;

        float apexY = startPos.y;
        float x = 0;
        float targetX = playerPos.x;
        float targetY = playerPos.y;
        float y = 0;
        landPos = target.transform.position;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            x = Mathf.Lerp(startPos.x, targetX, t);
            y = Mathf.Lerp(startPos.y, targetY, t);
            transform.position =new  Vector3(x,y,0);
            yield return null;
        }
        
    }
    public void Fall()
    {
        StartCoroutine(FallRoutine());
        SoundManager.Instance.PlaySFX(SFX.Boss_Fall);
    }

    private IEnumerator FallRoutine()
    {
        _bossState = BossState.Ground;
        elapsed = 0f;
        Vector3 fallStart = transform.position;
        fallStart.y = apexHeight;
        while (elapsed < JumpDuration/3)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / JumpDuration);
            transform.position = Vector3.Lerp(fallStart, landPos, t);
            yield return null;
        }
        transform.position = landPos;
    }
    public void BloodShot()
    {
        StartCoroutine(BloodShotRoutine());
        SoundManager.Instance.PlaySFX(SFX.Boss_Tear);
    }

    private IEnumerator BloodShotRoutine()
    {
        Vector3 headPos = transform.position + Vector3.up * 1.2f;
        Vector3 playerPos = target.transform.position;

        int tearCount = 12;
        float radius = 1.8f;
        float circleStep = 360f / tearCount;
        var tearsList = new List<MonstroTears>();

        // 1) 한 프레임에 12개 동시에 생성
        for (int i = 0; i < tearCount; i++)
        {
            // 2) 플레이어 주변 목표 지점
            float angle = circleStep * i;
            Vector2 offset = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * radius;
            Vector3 targetPos = playerPos + (Vector3)offset;

            // 3) 스폰 및 Launch 호출
            GameObject t = Instantiate(tears, headPos, Quaternion.identity);
            if (t.TryGetComponent<MonstroTears>(out var et))
            {
                et.LaunchTo(targetPos, enemy_projectileSpeed);
                tearsList.Add(et);
            }
        }
        yield return new WaitUntil(() =>
                                    tearsList.TrueForAll(et => et == null || et.HasArrived)
                                    );
    }

    public override void TakeDamage(int damage, Vector2? attackOrigin = null)
    {
        enemy_hp -= damage;
        if (enemy_hp <= 0) Die();

        onBossHpSlider?.Invoke(enemy_hp);

        if (attackOrigin.HasValue)
        {
            Vector2 dir = ((Vector2)transform.position - attackOrigin.Value).normalized;
            enemy_rb.AddForce(dir * boss_knockback, ForceMode2D.Impulse);
        }
    }

    public override void Die()
    {
        enemy_Collider.enabled = false;
        enabled = false;  

        enemy_Animator.SetTrigger(Dead);
        SoundManager.Instance.PlaySFX(SFX.Boss_Die);
        StartCoroutine(DeathShake());
    }


    private IEnumerator DeathShake()
    {
        SoundManager.Instance.PlayBGM(BGM.Boss_Win);
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

        yield return new WaitForSeconds(3f);
        enemy_Collider.enabled = false;
        SoundManager.Instance.PlayBGM(BGM.InGame);
        onDeath?.Invoke();
        Destroy(gameObject);
    }

}


