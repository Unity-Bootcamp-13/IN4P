using System.Threading.Tasks;
using UnityEngine;
public interface IDamageable
{
    void TakeDamage(int amount, Vector2? dir);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public RoomController roomcontroller;
    public bool isDead;
    public bool IsDead => isDead;

    public System.Action OnDeath; // 죽을 때 실행할 콜백

    public virtual async void Start()
    {
        await Task.Delay(500);
    }
    public int hp;
    public virtual void TakeDamage(int amount, Vector2? attackOrigin)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }

    }

    public virtual void Die()
    {
        Debug.Log($"[{name}] Enemy.Die() 호출! OnDeath 이전");
        OnDeath?.Invoke(); // 등록된 함수 호출 (예: CheckClearCondition)
        Debug.Log($"[{name}] OnDeath.Invoke() 리턴");
        Destroy(gameObject);
    }

}