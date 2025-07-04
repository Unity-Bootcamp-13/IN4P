using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{    
    public CharacterData characterData;

    // 픽업 아이템
    public int keyCount;
    public int bombCount = 1;

    public int hp;
    public float atk;
    public float atkSpeed;
    public float speed;
    public float atkRange;
    public float projectileSpeed;
    public int currentHp;

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

    public GameObject headObject;
    public GameObject bodyObject;
    public GameObject totalbodyObject;

    public Animator headAnimator;
    public Animator bodyAnimator;
    public Animator totalbodyAnimator;

    public SpriteRenderer bodySprite;

    public Transform eyes;
    public Transform leftEye;
    public Transform rightEye;

    private bool isAttacking;
    private int currentAttackDir = -1;


    private void Awake()
    {
        hp = characterData.PlayerHp;
        atk = characterData.Atk;
        atkSpeed = characterData.AtkSpeed;
        speed = characterData.Speed;
        atkRange = characterData.AtkRange;
        projectileSpeed = characterData.ProjectileSpeed;
        currentHp = hp;

        headObject = transform.GetChild(0).gameObject;
        bodyObject = transform.GetChild(1).gameObject;
        totalbodyObject = transform.GetChild(2).gameObject;

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
        attack.SetTears(projectileSpeed, atkRange, (int)atk);
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
        rid.linearVelocity = dir * speed;
    }

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (bombCount > 0)
        {
            bombCount--;
            GameObject go = Instantiate(bombPrefab);
            go.transform.position = this.transform.position;
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
            headAnimator.speed = atkSpeed;
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

    public void TakeDamage(int damage)
    {
        Debug.Log($"Player 데미지{damage} 입음");

        hp -= damage;
        
        if(hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
    }

}
