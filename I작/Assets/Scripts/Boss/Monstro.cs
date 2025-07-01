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
    public float jump_height = 1.5f;
    public float jumpDuration = 0.5f;
    private float jumpTimer = 0f;

    private BossState _bossState = BossState.Idle;

    public Transform player;
    public Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Coroutine _patternRoutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        float facing = spriteRenderer.flipX ? 1f : -1f;
        if (_bossState == BossState.Idle && player != null)
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

    //점프하면서 캐릭터 쪽으로 다가옴
    private IEnumerator LowJumpRoutine()
    {
        // 1) 애니메이터 트리거 (애니메이션 연동이 필요하다면)
        animator.SetTrigger("LowJump");

        Vector3 startPos = transform.position;
        Vector3 dir = (player.position - startPos).normalized;
        Vector3 endPos = startPos + dir * boss_MoveRange;

        float elapsed = 0f;

   
        // 2) 한 사이클(상승→하강) 동안 보간
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);

            // (a) 수평 보간
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            // (b) 수직 오프셋 (포물선)
            pos.y = Mathf.Lerp(startPos.y, endPos.y, t)
                  + jump_height * 4f * t * (1f - t);

            transform.position = pos;
            yield return null;
        }
        
        
    }


    //피 토하기
    public void BloodAttack()
    {
       
    }

    
    public void TakeDamage()
    {

    }

    //보스 사망
    public void bossDie()
    {
        
    }

}
