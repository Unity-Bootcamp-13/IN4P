using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public interface IAttackBehavior
{
    void Attack(string dir);
}

public class Attack : MonoBehaviour
{
    public GameObject tearsPrefab;
    public GameObject brimstonePrefab;

    public Transform Eyes;
    public Transform leftEye;
    public Transform rightEye;
    public Transform Mouse;
    private Animator HeadAnimator;
    private IAttackBehavior attackBehavior;

    private float playerRanged;
    private float playerAtkspeed;
    private float playerAtkDelay;
    private int playerAtk;
    private AttackDirection currentAttackDir;
    readonly private int poolSize = 50;

    private bool isAttacking;

    public static readonly int[] HeadHashes = new int[]
    {
        Animator.StringToHash("Head_Up"),
        Animator.StringToHash("Head_Down"),
        Animator.StringToHash("Head_Left"),
        Animator.StringToHash("Head_Right")
    };


    private void Awake()
    {
        HeadAnimator = GetComponent<Animator>();
        attackBehavior = new TearsAttack(this, tearsPrefab, leftEye, rightEye, poolSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("브림스톤(혈사포) 전환");
            SwitchToBrimstone();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("눈물 전환");
            SwitchToTears();
        }
        if (attackBehavior is BrimstoneAttack brimstone)
        {
            brimstone.UpdateCharging(Time.deltaTime);
        }
    }

    public void OnPress(AttackDirection dir, Quaternion rot)
    {
        if (isAttacking) return;

        isAttacking = true;
        currentAttackDir = dir;
        Eyes.rotation = rot;

        int hash = HeadHashes[(int)dir];
        HeadAnimator.SetBool(hash, true);
        float baseDelay = 2.73f;
        float animationSpeed = playerAtkDelay / baseDelay;
        HeadAnimator.speed = animationSpeed;

    }

    public void OnRelease(AttackDirection dir)
    {

        if (!isAttacking && dir != currentAttackDir)
            return;

          
        isAttacking = false;

        int hash = HeadHashes[(int)dir];
        HeadAnimator.SetBool(hash, false);
        if (attackBehavior is BrimstoneAttack brimstone)
        {
            attackBehavior.Attack(dir.ToString()); // Tears나 Brimstone 실행
        }
    }

    public void SetPlayerStats(float Atkspeed, float Range, int Damage, float AtkDelay)
    {
        playerAtk = Damage;
        playerRanged = Range;
        playerAtkspeed = Atkspeed;
        playerAtkDelay = AtkDelay;
        if (attackBehavior is TearsAttack tears)
        {
            tears.SetStats(Atkspeed, Range, Damage);
        }
        if (attackBehavior is BrimstoneAttack brimstoneAttack)
        {
            brimstoneAttack.SetStats(playerAtkDelay, Damage);
        }
    }

    public void AttackDirection(string dir)
    {
        attackBehavior.Attack(dir);
    }

    public void SwitchToBrimstone()
    {
        
        attackBehavior = new BrimstoneAttack(brimstonePrefab, Mouse, playerAtk, playerAtkDelay);
    }

    public void UpdateCharge(float deltaTime)
    {
        if (attackBehavior is BrimstoneAttack brimstone)
        {
            brimstone.UpdateCharging(deltaTime);
        }
    }

    public void SwitchToTears()
    {
        var newTears = new TearsAttack(this, tearsPrefab, leftEye, rightEye, 50);

        newTears.SetStats(playerAtkspeed, playerRanged, playerAtk);

        attackBehavior = newTears;
    }
    public void OnAttackEnd()
    {
        isAttacking = false;
    }
}
