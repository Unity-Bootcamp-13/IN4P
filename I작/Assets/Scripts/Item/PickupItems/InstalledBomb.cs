using UnityEngine;

public class InstalledBomb : MonoBehaviour
{
    public Collider2D bombCollider;
    public float explosionRadius = 2f;
    public int damage = 60;
    public LayerMask damageLayerMask;


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bombCollider.isTrigger = false;
        }
    }

    public void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayerMask);

        foreach (Collider2D hit in hits)
        {
            //IDamageable target = hit.GetComponent<IDamageable>();
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
