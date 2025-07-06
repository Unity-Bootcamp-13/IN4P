using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public interface IAttackBehavior
{
    void Attack(string dir);
}

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject tearsPrefab;
    [SerializeField] private GameObject brimstonePrefab;
    [SerializeField] private int poolSize = 50;

    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform Mouse;
    private IAttackBehavior attackBehavior;

    private float playerRanged;
    private float playerAtkspeed;
    private int playerAtk;


    private void Awake()
    {
        attackBehavior = new TearsAttack(this, tearsPrefab, leftEye, rightEye, 50);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("ºê¸²½ºÅæ(Ç÷»çÆ÷) ÀüÈ¯");
            SwitchToBrimstone();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("´«¹° ÀüÈ¯");
            SwitchToTears();
        }
    }

    public void SetPlayerStats(float Atkspeed, float range, int damage)
    {
        playerAtk = damage;
        playerRanged = range;
        playerAtkspeed = Atkspeed;
        if (attackBehavior is TearsAttack tears)
        {
            tears.SetStats(Atkspeed, range, damage);
        }
    }

    public void AttackDirection(string dir)
    {
        attackBehavior.Attack(dir);
    }

    public void SwitchToBrimstone()
    {
        attackBehavior = new BrimstoneAttack(brimstonePrefab, Mouse, playerAtk); // playerAtk´Â float
    }

    public void SwitchToTears()
    {
        var newTears = new TearsAttack(this, tearsPrefab, leftEye, rightEye, 50);

        newTears.SetStats(playerAtkspeed, playerRanged, playerAtk);

        attackBehavior = newTears;
    }
}
