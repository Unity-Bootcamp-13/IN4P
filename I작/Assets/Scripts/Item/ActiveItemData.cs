using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ACtiveItem", menuName = "SO/ItemData/ActiveData")]
public class ActiveItemData : ItemData, IStatModifier, IAttackModifier
{
    [SerializeField] private int gauge;
    [SerializeField] private float atkIncr;
    [SerializeField] private float atkScale;
    [SerializeField] private float atkSpeedIncr;
    [SerializeField] private float atkSpeedScale;
    [SerializeField] private float atkRangeIncr;
    [SerializeField] private float atkRangeScale;
    [SerializeField] private float tearsSpeedIncr;
    [SerializeField] private float tearsSpeedScale;
    [SerializeField] private float speedIncr;
    [SerializeField] private float currentHpIncr;
    [SerializeField] private float maxHpIncr;

    public Action<Player> activeAction;

    public int Gauge => gauge;
    public float AtkIncr => atkIncr;
    public float AtkScale => atkScale;
    public float AtkSpeedIncr => atkSpeedIncr;
    public float AtkSpeedScale => atkSpeedScale;
    public float AtkRangeIncr => atkRangeIncr;
    public float AtkRangeScale => atkRangeScale;
    public float TearsSpeedIncr => tearsSpeedIncr;
    public float TearsSpeedScale => tearsSpeedScale;
    public float SpeedIncr => speedIncr;
    public float CurrentHpIncr => currentHpIncr;
    public float MaxHpIncr => maxHpIncr;
    protected override void OnEnable()
    {
        base.OnEnable();
        SetItemType(ItemType.Active);
        activeAction += ModifyStats; 
        activeAction += ModifyAttack;
    }
    public void ModifyStats(Player player)
    {
        // 계산 공식 적용해야 함
        if (atkIncr != 0f)
            player.stats.atk += atkIncr;
        if (atkScale != 0f)
            player.stats.atk *= atkScale;

        if (atkSpeedIncr != 0f)
            player.stats.atkSpeed += atkSpeedIncr;
        if (atkSpeedScale != 0f)
            player.stats.atkSpeed *= atkSpeedScale;

        if (atkRangeIncr != 0f)
            player.stats.atkRange += atkRangeIncr;
        if (atkRangeScale != 0f)
            player.stats.atkRange *= atkRangeScale;

        if (tearsSpeedIncr != 0f)
            player.stats.projectileSpeed += tearsSpeedIncr;
        if (tearsSpeedScale != 0f)
            player.stats.projectileSpeed *= tearsSpeedScale;

        if (speedIncr != 0f)
            player.stats.speed += speedIncr;

        if (currentHpIncr != 0f)
            player.currentHp += currentHpIncr;
        if (maxHpIncr != 0f)
            player.stats.hp += maxHpIncr;
    }

    public void ModifyAttack(Player player)
    {
        Debug.Log("공격");

        // 아 진짜 이건 어떻게 하지
    }
}
