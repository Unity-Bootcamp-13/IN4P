using System.Collections.Generic;
using UnityEngine;

public class TearsAttack : IAttackBehavior
{
    private Attack owner;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;
    private Transform leftEye;
    private Transform rightEye;
    private bool launchPlace;

    private float speed;
    private float range;
    private int damage;

    public TearsAttack(Attack owner, GameObject prefab, Transform leftEye, Transform rightEye, int poolSize)
    {
        this.owner = owner;
        this.prefab = prefab;
        this.leftEye = leftEye;
        this.rightEye = rightEye;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject tear = GameObject.Instantiate(prefab);
            tear.SetActive(false);
            pool.Enqueue(tear);
        }
    }

    public void SetStats(float speed, float range, int damage)
    {
        this.speed = speed;
        this.range = range;
        this.damage = damage;

        foreach (var tear in pool)
        {
            var t = tear.GetComponent<Tears>();
            t.speed = speed;
            t.range = range;
            t.damage = damage;
        }
    }

    public void Attack(string dir)
    {
        GameObject go = GetTearFromPool();
        Tears tear = go.GetComponent<Tears>();
        tear.speed = speed;
        tear.range = range;
        tear.damage = damage;
        tear.dir = dir switch
        {
            "Up" => Vector2.up,
            "Down" => Vector2.down,
            "Left" => Vector2.left,
            "Right" => Vector2.right,
            _ => Vector2.right
        };

        go.transform.position = launchPlace ? leftEye.position : rightEye.position;
        launchPlace = !launchPlace;

        tear.SetReturnAction(() => ReturnToPool(go));
    }

    private GameObject GetTearFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject tear = pool.Dequeue();
            tear.SetActive(true);
            return tear;
        }

        return GameObject.Instantiate(prefab);
    }

    private void ReturnToPool(GameObject tear)
    {
        tear.SetActive(false);
        pool.Enqueue(tear);
    }
}