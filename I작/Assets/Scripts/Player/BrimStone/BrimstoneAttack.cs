using UnityEngine;

public class BrimstoneAttack : IAttackBehavior
{
    private GameObject prefab;
    private Transform firePoint;
    private float playerDamage;

    public BrimstoneAttack(GameObject prefab, Transform firePoint, float damage)
    {
        this.prefab = prefab;
        this.firePoint = firePoint;
        this.playerDamage = damage;
    }

    public void Attack(string dir)
    {
        Vector2 vectorDir = dir switch
        {
            "Up" => Vector2.up,
            "Down" => Vector2.down,
            "Left" => Vector2.left,
            "Right" => Vector2.right,
            _ => Vector2.right
        };

        GameObject go = GameObject.Instantiate(prefab, firePoint.position, Quaternion.identity);
        go.GetComponent<BrimstoneBeam>().direction = vectorDir;

        BrimstoneBeam beam = go.GetComponent<BrimstoneBeam>();
        beam.Initialize(vectorDir, playerDamage, firePoint);
    }
}