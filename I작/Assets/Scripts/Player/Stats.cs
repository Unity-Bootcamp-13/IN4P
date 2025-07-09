
using System;
using System.Collections.Generic;
using UnityEngine;
public enum StatType {MaxHP, CurrentHP, Bomb, Key }
public class Stats
{
    public int KeyCount;
    public int BombCount;
    public int Max_Hp;
    public int CurrentHp;
    public float Atk;
    public float AtkSpeed;
    public float Speed;
    public float AtkRange;
    public float ProjectileSpeed;

    private readonly int PickupMax = 99;
    
    public static event Action<StatType, int> OnChanged;

    public Stats(
        int keyCount,
        int bombCount,
        int maxHp,
        float atk,
        float atkSpeed,
        float speed,
        float atkRange,
        float projectileSpeed
    )
    {
        KeyCount = keyCount;
        BombCount = bombCount;
        Max_Hp = maxHp;
        CurrentHp = maxHp;
        Atk = atk;
        AtkSpeed = atkSpeed;
        Speed = speed;
        AtkRange = atkRange;
        ProjectileSpeed = projectileSpeed;

        OnChanged?.Invoke(StatType.Key, KeyCount);
        OnChanged?.Invoke(StatType.Bomb, BombCount);
        OnChanged?.Invoke(StatType.MaxHP, Max_Hp);
        OnChanged?.Invoke(StatType.CurrentHP, CurrentHp);
    }

    public Stats(Stats other)
    {
        KeyCount = other.KeyCount;
        BombCount = other.BombCount;
        Max_Hp = other.Max_Hp;
        CurrentHp = other.CurrentHp;
        Atk = other.Atk;
        AtkSpeed = other.AtkSpeed;
        Speed = other.Speed;
        AtkRange = other.AtkRange;
        ProjectileSpeed = other.ProjectileSpeed;

    }

    public Stats Apply(StatModifier SMF)
    {
        var s = new Stats(this);

        //식이 들어왔을때 판별 후 계산
        var operations = new Dictionary<ModifyType, Func<float, float, float>>()
        {
            { ModifyType.Addtion,     (value, amt) => value + amt },
            { ModifyType.Multiplication,   (value, amt) => value * amt }
        };


        switch (SMF.Target)
        {
            case ModifyTarget.KeyCount:
                s.KeyCount = (int)operations[SMF.ModifyType](s.KeyCount, SMF.Amount);
                s.KeyCount = Mathf.Min(s.KeyCount, PickupMax);
                OnChanged?.Invoke(StatType.Key, KeyCount);
                break;
            case ModifyTarget.BombCount:
                s.BombCount = (int)operations[SMF.ModifyType](s.BombCount, SMF.Amount);
                s.BombCount = Mathf.Min(s.BombCount, PickupMax);
                OnChanged?.Invoke(StatType.Bomb, BombCount);
                break;
            case ModifyTarget.Atk:
                s.Atk = operations[SMF.ModifyType](s.Atk, SMF.Amount);
                break;
            case ModifyTarget.AtkSpeed:
                s.AtkSpeed = operations[SMF.ModifyType](s.AtkSpeed, SMF.Amount);
                break;
            case ModifyTarget.AtkRange:
                s.AtkRange = operations[SMF.ModifyType](s.AtkRange, SMF.Amount);
                break;
            case ModifyTarget.Speed:
                s.Speed = operations[SMF.ModifyType](s.Speed, SMF.Amount);
                break;
            case ModifyTarget.ProjectileSpeed:
                s.ProjectileSpeed = operations[SMF.ModifyType](s.ProjectileSpeed, SMF.Amount);
                break;
            case ModifyTarget.MaxHp:
                s.Max_Hp = (int)operations[SMF.ModifyType](s.Max_Hp, SMF.Amount);
                s.Max_Hp = Mathf.Min(s.Max_Hp, 24);
                OnChanged?.Invoke(StatType.MaxHP, Max_Hp);
                break;
            case ModifyTarget.CurrentHp:
                s.CurrentHp = (int)operations[SMF.ModifyType](s.CurrentHp, SMF.Amount);
                s.CurrentHp = Mathf.Min(s.CurrentHp, Max_Hp);
                OnChanged?.Invoke(StatType.CurrentHP, CurrentHp);
                break;
        }

        return s;
    }

}

public class StatModifier
{
    public ModifyTarget Target { get; }
    public ModifyType ModifyType { get; }
    public float Amount { get; }

    public StatModifier(
        ModifyTarget target,
        ModifyType modifyType,
        float amount
    )
    {
        Target = target;
        ModifyType = modifyType;
        Amount = amount;
    }
}