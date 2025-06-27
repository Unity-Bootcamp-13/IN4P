using System;
using UnityEngine;

public class Tears : MonoBehaviour
{
    public float projectileSpeed;
    public Transform player;    
    public Vector2 dir;
    public Animator tearsAnimator;

    public float lifeTime = 10f;

    private float timer;
    private Action returnAction;


    private void OnEnable()
    {
        projectileSpeed = player.GetComponent<Player>().projectileSpeed;
        timer = lifeTime;
    }

    private void Update()
    {
        transform.position += (Vector3)dir * projectileSpeed * Time.deltaTime;
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            returnAction?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag 조사해서 몬스터면 터지는 걸로 변경
        tearsAnimator.SetBool("Pop", true);
        projectileSpeed = 0.5f;
    }

    void DestoryTears()
    {
        this.gameObject.SetActive(false);
    }

    public void SetReturnAction(Action returnToPool)
    {
        this.returnAction = returnToPool;
    }
}