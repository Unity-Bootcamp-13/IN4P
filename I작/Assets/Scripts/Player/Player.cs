using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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


    public Rigidbody2D rid;
    private Vector2 moveInput;

    [SerializeField] GameObject tearsPrefab;

    public Animator headAnimator;
    public Animator bodyAnimator;

    public SpriteRenderer bodySprite;

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
    private void Start()
    {
        //bodySprite = GetComponentInChildren<SpriteRenderer>();
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

    public void OnLeft(InputAction.CallbackContext context)
    {
        
    }

    public void CreateTears()
    {
        elapseTime = 0;

        GameObject go = Instantiate(tearsPrefab);
    }

   
}
