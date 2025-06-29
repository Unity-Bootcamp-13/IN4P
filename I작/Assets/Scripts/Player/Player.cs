using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{    
    public CharacterData characterData;
    public float currentHp;

    public Inventory inventory;
    public Stats stats;
    public Attack attack;

    private int h;
    private int v;
    private int isMove;

    private int HeadLeft;
    private int HeadRight;
    private int HeadUp;
    private int HeadDown;

    public Rigidbody2D rid;
    private Vector2 moveInput;

    [SerializeField] GameObject bombPrefab;

    public Animator headAnimator;
    public Animator bodyAnimator;

    public SpriteRenderer bodySprite;

    public Transform eyes;
    public Transform leftEye;
    public Transform rightEye;

    private bool isAttacking;
    private int currentAttackDir = -1;


    private void Awake()
    {
        inventory = new Inventory();
        stats = new Stats
        (
            characterData.Hp,
            characterData.Atk,
            characterData.AtkSpeed,
            characterData.Speed,
            characterData.AtkRange,
            characterData.ProjectileSpeed
        );
        currentHp = stats.hp;

        h = Animator.StringToHash("Horizontal");
        v = Animator.StringToHash("Vertical");
        isMove = Animator.StringToHash("IsMove");

        HeadLeft = Animator.StringToHash("Head_Left");
        HeadRight = Animator.StringToHash("Head_Right");
        HeadUp = Animator.StringToHash("Head_Up");
        HeadDown = Animator.StringToHash("Head_Down");
    }

    private void Start()
    {
        attack.UpdateTears(stats.projectileSpeed, stats.atkRange);
    }

    public void Update()
    {
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
        Vector3 dir = moveInput.normalized;
        rid.linearVelocity = dir * stats.speed;
    }

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        GameObject go = Instantiate(bombPrefab);
        go.transform.position = this.transform.position;
    }

    public void OnActive(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        inventory.UseActiveItem(this);
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
        OnAttack(context, Quaternion.Euler(0, 0, 180), HeadUp);
    }

    public void OnDownAttack(InputAction.CallbackContext context)
    {
        OnAttack(context, Quaternion.Euler(0, 0, 0), HeadDown);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        OnAttack(context, Quaternion.Euler(0, 0, -90), HeadLeft);
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        OnAttack(context, Quaternion.Euler(0, 0, 90), HeadRight);
    }

    private void OnAttack(InputAction.CallbackContext context, Quaternion rot, int dir)
    {
        if (context.started)
        {
            if (isAttacking)
                return;

            isAttacking = true;
            currentAttackDir = dir;

            eyes.rotation = rot;
            headAnimator.SetBool(dir, true);
            headAnimator.speed = stats.atkSpeed;
        }
        else if (context.canceled)
        {
            if (isAttacking && currentAttackDir == dir)
            {
                headAnimator.SetBool(dir, false);
                isAttacking = false;
                currentAttackDir = -1;
            }
        }
    }

    public void AcquireItem(ItemData item)
    {
        Debug.Log(item.Description);
        if (item is PassiveItemData passive)
        {
            inventory.AddPassiveItem(passive);

            if (item is IStatModifier statMod)
            {
                statMod.ModifyStats(this);
            }

            if (item is IAttackModifier attackMod)
            {
                attackMod.ModifyAttack(this);
            }

            //if (item is ISpecialAbility specialMod)
            //{
            //    specialMod.GrantAbility(this);
            //}
        }
        else if (item is ActiveItemData active)
        {
            inventory.EquipActiveItem(active);
        }
        else if (item is PickupItemData pickup)
        {
            if (pickup.PickupType == PickupType.Bomb)
            {
                inventory.bombCount++;
                Debug.Log("ÆøÅº °³¼ö Áõ°¡");
            }
            else if (pickup.PickupType == PickupType.Key)
            {
                inventory.keyCount++;
            }
            else if (pickup.PickupType == PickupType.Heart)
            {
                if (currentHp < stats.hp)
                    currentHp++;

                if (currentHp > stats.hp)
                    currentHp = stats.hp;
            }
        }
    }
}
