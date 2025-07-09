using System;
using UnityEngine;

public class Tears : MonoBehaviour
{
    public float speed;
    public float range;
    public Vector2 dir;
    public float timer;
    public int damage;

    public Animator tearsAnimator;
    private Action returnAction;
    


    private void OnEnable()
    {
        timer = range / speed;
    }

    private void Update()
    {
        transform.position += (Vector3)dir * speed * Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            returnAction?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag 조사해서 몬스터면 터지는 걸로 변경
        
        if (collision.TryGetComponent<Enemy>(out var monster) || collision.gameObject.CompareTag("Object"))
        {
            if (monster != null)
            {
                monster.TakeDamage(damage, dir);
            }
            
            tearsAnimator.SetTrigger("Pop");
            speed = 0.1f;
        }
        else
        {
            return;
        }
    }

    void StartTearsSound()
    {
        SoundManager.Instance.PlaySFX(SFX.TearFire);
    }

    void DestoryTears()
    {
        SoundManager.Instance.PlaySFX(SFX.Tear);
        returnAction?.Invoke();
    }

    public void SetReturnAction(Action returnToPool)
    {
        this.returnAction = returnToPool;
    }
}