using System;
using UnityEngine;

public class Tears : MonoBehaviour
{
    public float speed;
    public float range;
    public Vector2 dir;
    public float timer;
    public int damage;

    public Animator tearsAnimator;
    private Action returnAction;
    


    private void OnEnable()
    {        
        //timer = range / speed;
        timer = 5;
    }

    private void Update()
    {
        transform.position += (Vector3)dir * speed * Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            returnAction?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag 조사해서 몬스터면 터지는 걸로 변경
        if(collision.gameObject.tag == "Player")
        {
            return;
        }


        if (collision.gameObject.tag == "Monster")
        {
            ContactMonster monster = collision.GetComponent<ContactMonster>();

            if(monster != null)
            {
                monster.TakeDamage(damage);
            }
        }

        tearsAnimator.SetTrigger("Pop");
        speed = 0.1f;
    }

    void DestoryTears()
    {
        Debug.Log("눈물 삭제");
        this.gameObject.SetActive(false);
    }

    public void SetReturnAction(Action returnToPool)
    {
        this.returnAction = returnToPool;
    }
}