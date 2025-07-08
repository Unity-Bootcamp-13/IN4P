using System.Threading.Tasks;
using UnityEngine;
public interface IDamageable
{
    void TakeDamage(int amount,Vector2? dir);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public virtual async void Start()
    {
        await Task.Delay(500);
    }
    public int hp;
    public virtual void TakeDamage(int amount,Vector2? attackOrigin)
    {
        hp -= amount;
        if(hp <=0) Die();
        
    }

    public virtual void Die()
    { }

}