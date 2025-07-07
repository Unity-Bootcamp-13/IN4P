using System.Collections;
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
    AttackDirection attackDirection;


    private int h;
    private int v;
    private int isMove;

   
    public Attack attack;
    public Rigidbody2D rid;
    private Vector2 moveInput;

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

    public StatPassiveItem statPassiveItem;


    private void Awake()
    {
        hp = characterData.PlayerHp;
        atk = characterData.Atk;
        atkSpeed = characterData.AtkSpeed;
        speed = characterData.Speed;
        atkRange = characterData.AtkRange;
        projectileSpeed = characterData.ProjectileSpeed;
        currentHp = hp;

        bodyObject = transform.GetChild(1).gameObject;
        totalbodyObject = transform.GetChild(2).gameObject;

        h = Animator.StringToHash("Horizontal");
        v = Animator.StringToHash("Vertical");
        isMove = Animator.StringToHash("IsMove");

        
    }

    private void Start()
    {
        attack.SetPlayerStats(projectileSpeed, atkRange, (int)atk ,atkSpeed);
    }

    public void Update()
    {
        if (isHurt)
        {
            return;
        }

        if (isDead)
        {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
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
        attackDirection = AttackDirection.Up;
        OnAttack(context);

    }

    public void OnDownAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Down;
        OnAttack(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Left;
        OnAttack(context);
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        attackDirection = AttackDirection.Right;
        OnAttack(context);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            attack.OnPress(attackDirection);
        if (context.canceled)
            attack.OnRelease(attackDirection);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Player 데미지{damage} 입음");

        hp -= damage;

        isHurt = true;

        totalbodyObject.SetActive(true);
        aquireItemObject.SetActive(false);
        totalbodyAnimator.SetTrigger("IsHurt");

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isHurt = false;
        isDead = true;
        StartCoroutine(DeadAnim());
    }

    // 패시브 아이템 획득
    public void ApplyItem(Sprite itemsprite)
    {
        AquireItemAnim(itemsprite);
    }

    IEnumerator DeadAnim()
    {
        float elapsedTime = 0f;
        float shakeDurtaion = 2.5f;

        while (elapsedTime < shakeDurtaion)
        {
            spotLightObject.SetActive(true);
            float shakePosX = Mathf.PingPong(Time.time * shakeSpeed, 0.2f);
            transform.position = new Vector3(shakePosX, transform.position.y, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DeadAnimFinish();
    }

    // 아이템 획득 시
    private void AquireItemAnim(Sprite itemsprite)
    {
        isItem = true;

        totalbodyObject.SetActive(true);
        aquireItemObject.SetActive(true);
        aquireItemObject.GetComponent<SpriteRenderer>().sprite = itemsprite;
        totalbodyAnimator.SetTrigger("IsItem");
    }

    public void HurtAnimFinish()
    {
        isHurt = false;
        Debug.Log("피격 종료");
    }

    public void DeadAnimFinish()
    {
        isDead = false;
        gameObject.SetActive(false);
        Debug.Log("사망 종료");
    }

    public void AquireItemFinish()
    {
        isItem = false;
        Debug.Log("아이템 획득 종료");
    }

}
