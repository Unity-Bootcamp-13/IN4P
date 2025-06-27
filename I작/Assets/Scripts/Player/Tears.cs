using UnityEngine;

public class Tears : MonoBehaviour
{
    public float projectileSpeed;
    public Transform player;    
    public Vector2 dir;
    public Animator tearsAnimator;

    private void Start()
    {
        projectileSpeed = player.GetComponent<Player>().projectileSpeed;
        Debug.Log(dir);
    }

    private void Update()
    {
        transform.position += (Vector3)dir * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tearsAnimator.SetBool("IsWall", true);
        projectileSpeed = 0.5f;
    }

    void DestoryTears()
    {
        this.gameObject.SetActive(false);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    tearsAnimator.SetBool("IsWall", true);
    //    collision.gameObject.SetActive(false);
    //}
}