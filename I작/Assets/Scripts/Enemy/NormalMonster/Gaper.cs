using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Gaper : Enemy
{
    public GameObject GusherPrefab;


    private void Update()
    {
        ChasePlayerMovement();
    }


    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
    }
    public void ChasePlayerMovement()
    {
        Vector2 direciton = (target.transform.position - transform.position).normalized;
        enemy_rb.MovePosition(enemy_rb.position + direciton * enemy_speed * Time.deltaTime);
    }


    public override void Die()
    {
        StartCoroutine(Died());
    }

    private IEnumerator Died()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand > 0)
        {

            yield return StartCoroutine(SpawnGusher());
        }
        
        // this.gameObject.SetActive(false);
        base.Die();
    }

    private IEnumerator SpawnGusher()
    {
        
        GameObject gusher = Instantiate(GusherPrefab, transform.position, Quaternion.identity);
        var controller = roomcontroller as NormalRoomController;
        if (controller == null)
        {
            Debug.LogError($"[{name}] roomcontroller�� �Ҵ���� �ʾҰų� NormalRoomController�� �ƴմϴ�.");
            yield break;
        }
        controller.monsterCount++;
        Enemy gusherEnemy = gusher.GetComponentInChildren<Enemy>();
        if (gusherEnemy != null)
        {
            gusherEnemy.roomcontroller = controller;
            gusherEnemy.OnDeath += controller.CheckClearCondition;
            Debug.Log("Gusher�� OnDeath ���� �Ϸ�.");
        }
        Debug.Log($"[SpawnGusher] monsterCount ����. ����: {controller.monsterCount}");
        yield return null;
    }
}
