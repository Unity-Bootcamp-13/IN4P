using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackDirection
{
    Up,
    Down,
    Left,
    Right
}

public class Player : MonoBehaviour
{
    public float hp = 3.0f;
    public float atk = 1.0f;
    public float atkSpeed = 1.0f;
    public float speed = 1.0f;
    public float atkRange = 1.0f;
    public float projectileSpeed = 7.5f;
    public float elapseTime = 0;

    private int h;
    private int v;
    private int isMove;

    private int HeadLeft;
    private int HeadRight;
    private int HeadUp;
    private int HeadDown;

    private bool isLooking;

    public Rigidbody2D rid;
    private Vector2 moveInput;

    [SerializeField] GameObject tearsPrefab;

    public Animator headAnimator;
    public Animator bodyAnimator;

    public SpriteRenderer bodySprite;

    public Transform leftEye;
    public Transform rightEye;

    bool launchPlace;


    Vector2[] directions = new Vector2[]
    {
        new Vector2(1.0f, 0f), // 오른쪽
        new Vector2(-1.0f, 0f), // 왼족
        new Vector2(0f, -1.0f), // 아래
        new Vector2(0f, 1.0f) // 위
    };

    private void Awake()
    {
        h = Animator.StringToHash("Horizontal");
        v = Animator.StringToHash("Vertical");
        isMove = Animator.StringToHash("IsMove");

        HeadLeft = Animator.StringToHash("Head_Left");
        HeadRight = Animator.StringToHash("Head_Right");
        HeadUp = Animator.StringToHash("Head_Up");
        HeadDown = Animator.StringToHash("Head_Down");
    }


    public void Update()
    {
        if (moveInput.magnitude > 0)
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

    public void KeyDownLook(AttackDirection directionType)
    {
        Debug.Log($"KeyDownLook 호출: {directionType}");
        switch (directionType)
        {
            case AttackDirection.Right:
                headAnimator.SetBool(HeadRight, true);
                CreateTears(0);
                break;
            case AttackDirection.Left:
                headAnimator.SetBool(HeadLeft, true);
                CreateTears(1);
                break;
            case AttackDirection.Up:
                headAnimator.SetBool(HeadUp, true);
                CreateTears(2);
                break;
            case AttackDirection.Down:
                headAnimator.SetBool(HeadDown, true);
                CreateTears(3);
                break;      
        }
    }

    public void KeyUpLook()
    {
        headAnimator.SetBool(HeadRight, false);
        headAnimator.SetBool(HeadLeft, false);
        headAnimator.SetBool(HeadUp, false);
        headAnimator.SetBool(HeadDown, false);
    }
    
    public void CreateTears(int dir)
    {
        elapseTime = 0;

        GameObject go = Instantiate(tearsPrefab);

        if (launchPlace)
        {
            go.transform.position = leftEye.position;
            launchPlace = false;
        }
        else
        {
            go.transform.position = rightEye.position;
            launchPlace = true;
        }

        switch(dir)
        {
            case 0:
                go.GetComponent<Tears>().dir = Vector3.right; 
                break;
            case 1:
                go.GetComponent<Tears>().dir = Vector3.left;
                break;
            case 2:
                go.GetComponent<Tears>().dir = Vector3.up;
                break;
            case 3:
                go.GetComponent<Tears>().dir = Vector3.down;
                break;
        }    
    }
}
