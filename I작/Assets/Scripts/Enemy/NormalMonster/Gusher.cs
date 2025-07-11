using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gusher : Enemy
{
    
    Vector2 MoveDir;
    int movDirX;
    int movDirY;
    public GameObject tearsPrefab;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(MoveRandom());
        StartCoroutine(tearsSpawnRoutine());
    }

    private void FixedUpdate()
    {
        enemy_rb.MovePosition(enemy_rb.position + MoveDir * enemy_speed * Time.deltaTime);
    }

    IEnumerator MoveRandom()
    {
        while (true)
        {
            movDirX = Random.Range(-1, 2);
            movDirY = Random.Range(-1, 2);
            
            while (movDirX == 0 && movDirY == 0)
            {
                movDirX = Random.Range(-1, 2);
                movDirY = Random.Range(-1, 2);
                
            }

            MoveDir = new Vector2(movDirX, movDirY).normalized;

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator tearsSpawnRoutine()
    {
        while(true)
        {
            tearsSpawn();
            yield return new WaitForSeconds(enemy_atkSpeed);
        }
        
    }

    void tearsSpawn()
    {
        Vector2 dir = new Vector2(movDirX, movDirY);
        float angleOffset = Random.Range(-10f, 10f);
        Quaternion rot = Quaternion.AngleAxis(angleOffset, Vector3.forward);
        Vector2 rotatedDir = rot * dir;

        GameObject tears = Instantiate(tearsPrefab,transform.position, rot);
        NormalTears et = tears.GetComponent<NormalTears>();
        et.SetTears(enemy_projectileSpeed, enemy_atkRange, (int)enemy_atk, rotatedDir);
    }

    
    public override void Die()
    {
        
        StartCoroutine(DieAnimation());
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        base.Die();
    }
    
    
}
