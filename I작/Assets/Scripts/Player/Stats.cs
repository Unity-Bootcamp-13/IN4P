public class Stats
{
    public int playerHp;
    public float atk;
    public float atkSpeed;
    public float speed;
    public float atkRange;
    public float projectileSpeed;

    public Stats(int hp, float atk, float atkSpeed, float speed, float atkRange, float projectileSpeed)
    {
        this.playerHp = hp;
        this.atk = atk;
        this.atkSpeed = atkSpeed;
        this.speed = speed;
        this.atkRange = atkRange;
        this.projectileSpeed = projectileSpeed;
    }
}