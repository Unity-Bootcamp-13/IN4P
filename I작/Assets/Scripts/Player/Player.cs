using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
public enum AttackDirection
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}
public class Player : MonoBehaviour
{
    [SerializeField] ItemServiceSO itemServiceSO;
    [SerializeField] CharacterData characterData;
    [SerializeField] PlayerInput playerInput;

    public Stats stats;
    private Stats oldStats;
    public List<int> passiveItems = new List<int>();
    private int activeItem = -1; // 액티브 아이템 없는 상태
    public int currentGauge;

    private int h;
    private int v;
    private int isMove;

    private AttackDirection attackDirection;
    private Quaternion rot;
    private Vector2 moveInput;

    public Attack attack;
    public Rigidbody2D rid;
    

    [SerializeField] GameObject bombPrefab;

    public GameObject bodyObject;
    public GameObject totalbodyObject;
    public GameObject spotLightObject;
    public GameObject aquireItemObject;


    public Animator bodyAnimator;
    public Animator totalbodyAnimator;

    public SpriteRenderer bodySprite;

    // 애니메이션 상태 전환
    public bool isItem = false;
    public bool isHurt = false;
    public bool isDead = false;
    private float shakeSpeed = 5f;


    private void Awake()
    {
        playerInput.enabled = true;

        stats = new Stats
        (
            characterData.KeyCount,
            characterData.BombCount,
            characterData.PlayerHp,
            characterData.Atk,
            characterData.AtkSpeed,
            characterData.Speed,
            characterData.AtkRange,
            characterData.ProjectileSpeed
        );

        bodyObject = transform.GetChild(1).gameObject;
        totalbodyObject = transform.GetChild(2).gameObject;

        h = Animator.StringToHash("Horizontal");
        v = Animator.StringToHash("Vertical");
        isMove = Animator.StringToHash("IsMove");


    }

    private void Start()
    {
        attack.SetPlayerStats(stats.ProjectileSpeed, stats.AtkRange, (int)stats.Atk ,stats.AtkSpeed);
    }

    public void Update()
    {
        if (isHurt)
        {
            return;
        }

        if (isDead)
        {
            playerInput.enabled = false;
            moveInput = Vector2.zero;
            return;
        }

        if (moveInput != Vector2.zero)
        {
            bodyAnimator.SetBool(isMove, true);
            bodyAnimator.SetFloat(h, moveInput.x);
            bodyAnimator.SetFloat(v, moveInput.y);
        }
        else
        {
            bodyAnimator.SetBool(isMove, false);
        }

       

    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rid.linearVelocity = Vector2.zero;
            return;
        }

        Vector3 dir = moveInput.normalized;
        rid.linearVelocity = dir * stats.Speed;
        attack.SetPlayerStats(stats.ProjectileSpeed, stats.AtkRange, (int)stats.Atk, stats.AtkSpeed);
        
       
    }

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (stats.BombCount > 0)
        {
            stats.BombCount--;
            GameObject go = Instantiate(bombPrefab);
            go.transform.position = this.transform.position;
            go.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x < 0)
        {
            bodySprite.flipX = true;
        }
        else
        {
            bodySprite.flipX = false;
        }
    }

    public void OnUpAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Up;
        rot = Quaternion.Euler(0f, 0f, 180f);
        OnAttack(context);

    }

    public void OnDownAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Down;
        rot = Quaternion.Euler(0f, 0f, 0f);
        OnAttack(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Left;
        rot = Quaternion.Euler(0f, 0f, -90f);
        OnAttack(context);
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Right;
        rot = Quaternion.Euler(0f, 0f, 90f);
        OnAttack(context);
    }

    public void OnActive(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        UseActiveItem();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            attack.OnPress(attackDirection,rot);
        if (context.canceled)
            attack.OnRelease(attackDirection);
    }

   
    public void TakeDamage(int damage)
    {
        stats.ChangeHp(-damage);

        SoundManager.Instance.PlaySFX(SFX.Damage);

        isHurt = true;

        totalbodyObject.SetActive(true);
        aquireItemObject.SetActive(false);
        totalbodyAnimator.SetTrigger("IsHurt");

        if (stats.CurrentHp <= 0)
        {
            isHurt = false;
            SoundManager.Instance.PlaySFX(SFX.Dead);
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        StartCoroutine(DeadAnim());
    }

    IEnumerator DeadAnim()
    {
        float elapsedTime = 0f;
        float shakeDurtaion = 2.5f;

        Vector3 currentPosition = transform.position;

        while (elapsedTime < shakeDurtaion)
        {
            spotLightObject.SetActive(true);
            float shakePosX = Mathf.PingPong(Time.time * shakeSpeed, 0.2f);
            transform.position = new Vector3(currentPosition.x + shakePosX, transform.position.y, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DeadAnimFinish();
    }

    // 아이템 획득 시
    public void AcquireItem(int id, Sprite itemSprite)
    {
        AquireItemAnim(itemSprite);
        List<StatModifier> statModifiers = itemServiceSO.itemService.GetStatModifier(id);
        ItemType itemType = itemServiceSO.itemService.GetItemType(id);

        if (itemType == ItemType.Passive || itemType == ItemType.Pickup)
        {
            passiveItems.Add(id);

            for (int i = 0; i < statModifiers.Count; i++)
            {
                Stats newStats = stats.Apply(statModifiers[i]);
                if (newStats.CurrentHp <= 0)
                {
                    Die();
                }
                stats = newStats;
            }
        }
        else if (itemType == ItemType.Active)
        {
            DropActiveItem();
            activeItem = id;
            currentGauge = itemServiceSO.itemService.GetItemGauge(id);
        }
    }

    public void AquireItemAnim(Sprite itemsprite)
    {
        isItem = true;

        totalbodyObject.SetActive(true);
        aquireItemObject.SetActive(true);
        aquireItemObject.GetComponent<SpriteRenderer>().sprite = itemsprite;
        totalbodyAnimator.SetTrigger("IsItem");
    }

    private void DropActiveItem()
    {
        if (activeItem < 0)
            return;

        GameObject prefab = Resources.Load<GameObject>("Prefab/ItemPrefab");
        GameObject itemGo = GameObject.Instantiate(prefab, transform.position + Vector3.down * 2f, Quaternion.identity);
        var item = itemGo.GetComponent<Item>();
        item.itemId = activeItem;
        item.spritePath = itemServiceSO.itemService.GetSpritePath(activeItem);

        activeItem = -1;
    }

    private void UseActiveItem()
    {
        if (activeItem < 0)
        {
            return;
        }

        if (currentGauge < itemServiceSO.itemService.GetItemGauge(activeItem))
        {
            return;
        }

        oldStats = stats;

        List<StatModifier> statModifiers = itemServiceSO.itemService.GetStatModifier(activeItem);

        for (int i = 0; i < statModifiers.Count; i++)
        {
            Stats newStats = stats.Apply(statModifiers[i]);
            if (newStats.CurrentHp <= 0)
            {
                Die();
            }
            stats = newStats;
        }
        currentGauge = 0;
    }

    public void RevertStats()
    {
        if (oldStats != null)
        {
            int calculatedHp = stats.CurrentHp;
            stats = oldStats;
            stats.CurrentHp = calculatedHp;
            oldStats = null;
        }
    }

    public void HurtAnimFinish()
    {
        isHurt = false;
    }

    public void DeadAnimFinish()
    {
        isDead = false;
        gameObject.SetActive(false);
    }

    public void AquireItemFinish()
    {
        isItem = false;
    }

}
