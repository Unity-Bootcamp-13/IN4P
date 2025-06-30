public interface IStatModifier
{
    public float AtkIncr { get; }
    public float AtkScale { get; }
    public float AtkSpeedIncr { get; }
    public float AtkSpeedScale { get; }
    public float AtkRangeIncr { get; }
    public float AtkRangeScale { get; }
    public float TearsSpeedIncr { get; }
    public float TearsSpeedScale { get; }
    public float SpeedIncr { get; }
    public float CurrentHpIncr { get; }
    public float MaxHpIncr { get; }


    void ModifyStats(Player player);
}