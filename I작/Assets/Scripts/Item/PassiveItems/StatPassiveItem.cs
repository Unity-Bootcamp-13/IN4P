using UnityEngine;

public enum TargetStat
{
    Attack,
    AttackRate,
    AttackSpeed,
    AttackSpeedRate,
    AttackRange,
    AttackRangeRate,
    Speed,
    MaxHp,
    CurrentHp,
    ProjectileSpeed
}

public class StatPassiveItem : MonoBehaviour
{
    public Sprite itemIcon;

    public TargetStat[] targetStat;
    public float[] valueToApply;


    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            ApplyEffect(collider.GetComponent<Player>());

             collider.GetComponent<Player>().ApplyItem(itemIcon);

            Destroy(gameObject);
        }
    }

    private void ApplyEffect(Player player)
    {
        for (int i = 0; i < targetStat.Length; i++)
        {
            switch(targetStat[i])
            {
                case TargetStat.Attack:
                    player.atk += valueToApply[i];
                    break;
                case TargetStat.AttackRate:
                    player.atk *= valueToApply[i];
                    break;
                case TargetStat.AttackSpeed: 
                    player.atkSpeed += valueToApply[i];
                    break;
                case TargetStat.AttackSpeedRate:
                    player.atkSpeed *= valueToApply[i];
                    break;
                case TargetStat.AttackRange:
                    player.atkRange += valueToApply[i];
                    break;
                case TargetStat.AttackRangeRate: 
                    player.atkRange *= valueToApply[i];
                    break;
                case TargetStat.Speed: 
                    player.speed += valueToApply[i];
                    break;
                case TargetStat.MaxHp:
                    player.Max_hp += (int)valueToApply[i];
                    break;
                case TargetStat.CurrentHp:
                    player.currentHp += (int)valueToApply[i];
                    break;
                case TargetStat.ProjectileSpeed: 
                    player.projectileSpeed += valueToApply[i];
                    break;
                default:
                    break;
            }
        }
    }
}
