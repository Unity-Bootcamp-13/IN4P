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
    public int coolDown;
    public string boss_name = "Monstro";
    public int boss_hp = 250;
    public float boss_speed = 2;
    public float boss_knockback;
    public int boss_oneDamage = 2;
    public int boss_halfDamage = 1;
    public float boss_attackSpeed = 5f;
    public float boss_MoveRange = 4f;
    public float jump_height = 1.5f;
    public float jumpDuration = 0.5f;
    private float jumpTimer = 0f;

    private BossState _bossState = BossState.Idle;

    public Transform player;
    public Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Coroutine _jumpRoutine = null;

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
            _bossState = BossState.LowJump;
            _jumpRoutine = StartCoroutine(LowJumpRoutine());
        }

    }

    //������ ���� ���� ��
    public void GetDamage()
    {
        

    }
    private void StartLowJump()
    {
        if (_jumpRoutine != null)
        {
            StopCoroutine(_jumpRoutine);
        }
        _bossState = BossState.Idle;
        _jumpRoutine = StartCoroutine(LowJumpRoutine());

    }


    private IEnumerator DelayLow(float delay)
    {
        yield return new WaitForSeconds(delay);
    }



    //�����ϸ鼭 ĳ���� ������ �ٰ���
    private IEnumerator LowJumpRoutine()
    {
        // 1) �ִϸ����� Ʈ���� (�ִϸ��̼� ������ �ʿ��ϴٸ�)
        animator.SetTrigger("LowJump");

        Vector3 startPos = transform.position;
        Vector3 dir = (player.position - startPos).normalized;
        Vector3 endPos = startPos + dir * boss_MoveRange;

        float elapsed = 0f;

   
        // 2) �� ����Ŭ(��¡��ϰ�) ���� ����
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
        transform.position = new Vector3(endPos.x, startPos.y, endPos.z);
        _bossState = BossState.Idle;
        _jumpRoutine = null;
    }


    //�� ���ϱ�
    public void BloodAttack()
    {
       
    }
    public void TakeDamage()
    {

    }

    //���� ���
    public void bossDie()
    {
        
    }

}
