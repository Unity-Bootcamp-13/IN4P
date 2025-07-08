using UnityEngine;

public class BrimstoneAttack : IAttackBehavior
{
    private GameObject prefab;
    private Transform firePoint;
    private float playerDamage;

    private float requiredChargeTime;
    private float currentCharge;
    readonly private float minChargeTime = 0.5f;
    readonly private float maxChargeTime = 2.5f;
    private bool isCharging;
    private Vector2 currentDirection;

    public BrimstoneAttack(GameObject prefab, Transform firePoint,float damage , float delay)
    {
        this.prefab = prefab;
        this.firePoint = firePoint;
        this.playerDamage = damage;
        float chargeTime = 2.73f/ delay;

        requiredChargeTime = Mathf.Clamp(chargeTime, minChargeTime, maxChargeTime);

    }
    public void SetStats(float delay ,int damage)
    {
        this.playerDamage = damage;
        float chargeTime = 2.73f / delay;
        requiredChargeTime = Mathf.Clamp(chargeTime, minChargeTime, maxChargeTime);


    }

    public void Attack(string dir)
    {
        currentDirection = dir switch
        {
            "Up" => Vector2.up,
            "Down" => Vector2.down,
            "Left" => Vector2.left,
            "Right" => Vector2.right,
            _ => Vector2.right
        };

        currentCharge = 0f;
        isCharging = true;
    }

    public void UpdateCharging(float deltaTime)
    {
        if (!isCharging) return;

        currentCharge += deltaTime;

        if (currentCharge >= requiredChargeTime)
        {
            Fire();
            isCharging = false;
        }
    }

    private void Fire()
    {
        GameObject go = GameObject.Instantiate(prefab, firePoint.position, Quaternion.identity);
        BrimstoneBeam beam = go.GetComponent<BrimstoneBeam>();
        beam.Initialize(currentDirection, playerDamage, firePoint);
    }

   
}