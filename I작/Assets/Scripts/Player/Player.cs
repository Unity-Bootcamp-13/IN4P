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

    public int Max_hp;
    public float atk;
    public float atkSpeed;
    public float speed;
    public float atkRange;
    public float projectileSpeed;
    public int currentHp;
    

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

    public StatPassiveItem statPassiveItem;


    private void Awake()
    {
        Max_hp = characterData.PlayerHp;
        atk = characterData.Atk;
        atkSpeed = characterData.AtkSpeed;
        speed = characterData.Speed;
        atkRange = characterData.AtkRange;
        projectileSpeed = characterData.ProjectileSpeed;
        currentHp = Max_hp;

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
        attack.SetPlayerStats(projectileSpeed, atkRange, (int)atk, atkSpeed);
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

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            attack.OnPress(attackDirection,rot);
        if (context.canceled)
            attack.OnRelease(attackDirection);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        isHurt = true;

        totalbodyObject.SetActive(true);
        aquireItemObject.SetActive(false);
        totalbodyAnimator.SetTrigger("IsHurt");

        if (currentHp <= 0)
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
