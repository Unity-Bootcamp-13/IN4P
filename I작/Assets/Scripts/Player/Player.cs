using System;
using JetBrains.Annotations;
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

    private bool isLooking;

    public Rigidbody2D rid;
    private Vector2 moveInput;

    [SerializeField] GameObject tearsPrefab;
    [SerializeField] GameObject bombPrefab;

    public Animator headAnimator;
    public Animator bodyAnimator;

    public SpriteRenderer bodySprite;

    public Transform leftEye;
    public Transform rightEye;

    bool launchPlace;


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
        elapseTime += Time.deltaTime;

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

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        GameObject go = Instantiate(bombPrefab);
        go.transform.position = this.transform.position;
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
        OnAttack(context, HeadUp);
    }

    public void OnDownAttack(InputAction.CallbackContext context) 
    {
        OnAttack(context, HeadDown);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        OnAttack(context, HeadLeft);
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        OnAttack(context, HeadRight);
    }

    private void OnAttack(InputAction.CallbackContext context, int dir )
    {
        string path = context.control.path;
        headAnimator.speed = atkSpeed;

        if (context.started)
        {
            headAnimator.SetBool(dir, true);
        }
        else if (context.canceled)
        {
            headAnimator.SetBool(dir, false);
        }
    }
}
