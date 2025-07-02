using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class InstalledBomb : MonoBehaviour
{
    public Animator bombAnimator;
    public int monsterDamage = 60;
    public int characterDamage = 1;

    public Collider2D atkRange;

    private void Start()
    {
        bombAnimator.SetTrigger("Bomb");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().currentHp -= characterDamage;
        }

        if (other.tag == "Monster")
        {
            // 몬스터 데미지
        }

        if (other.tag == "Obstacle")
        {
            // 장애물 파괴
        }
    }

    void Explosion()
    {
        atkRange.enabled = true;
    }

    void DestroyBomb()
    {
        Destroy(this.gameObject);
    }
}
