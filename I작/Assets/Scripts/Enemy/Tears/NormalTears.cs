using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;



public class NormalTears: MonoBehaviour
{
    public float speed;
    public float range;
    public int damage;
    public Vector3 target;
    public float timer=0;
    public float lifetime = 5f;
    
    Animator tearsAnimator;
    Rigidbody2D rb;
    private void Awake()
    {
        tearsAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > lifetime)
        {
            ExplodeAndDestroyTears();
        }

    }

    void ExplodeAndDestroyTears()
    {
        rb.linearVelocity = Vector2.zero;
        tearsAnimator.SetTrigger("Pop");
       
        Destroy(gameObject,0.2f);
        enabled = false;
    }

    public void SetTears(float Enemy_speed, float Enemy_range, int Enemy_damage, Vector2 dir)
    {

        Vector2 direction = dir.normalized;
        speed = Enemy_speed;
        range = Enemy_range;
        damage = Enemy_damage;
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
                speed = 0f;
                ExplodeAndDestroyTears();
            }
        }
    }
}




