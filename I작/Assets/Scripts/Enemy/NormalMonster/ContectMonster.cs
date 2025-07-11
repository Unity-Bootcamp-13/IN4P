using System.Collections;
using UnityEngine;

public class ContectMonster : Enemy
{
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

    public override void Die()
    {
        enemy_Animator.SetTrigger("IsDead");
        StartCoroutine(DieAnimation());
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySFX(SFX.Monster_Die);
        base.Die();
    }
}
