using UnityEngine;

public class BrimstoneAttack : IAttackBehavior
{
    private GameObject prefab;
    private Transform firePoint;
    private float playerDamage;

    private float requiredChargeTime;
    private float currentCharge;
    private bool isCharging;
    private Vector2 currentDirection;

    public BrimstoneAttack(GameObject prefab, Transform firePoint,float damage , float delay)
    {
        this.prefab = prefab;
        this.firePoint = firePoint;
        this.playerDamage = damage;

        requiredChargeTime = 1.0f / delay;
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
        Debug.Log($"[Brimstone] 차지 시작. 방향: {currentDirection}");
        isCharging = true;
    }

    public void UpdateCharging(float deltaTime)
    {
        if (!isCharging) return;

        currentCharge += deltaTime;

        if (Mathf.FloorToInt(currentCharge * 10) % 5 == 0)
        {
            Debug.Log($"[Brimstone] 차징 중... {currentCharge:F2}/{requiredChargeTime:F2}초");
        }

        if (currentCharge >= requiredChargeTime)
        {
            Fire();
            isCharging = false;
        }
    }

    private void Fire()
    {
        Debug.Log("[Brimstone] 발사 완료!");
        GameObject go = GameObject.Instantiate(prefab, firePoint.position, Quaternion.identity);
        BrimstoneBeam beam = go.GetComponent<BrimstoneBeam>();
        beam.Initialize(currentDirection, playerDamage, firePoint);
    }

   
}