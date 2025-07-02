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
            // ���� ������
        }

        if (other.tag == "Obstacle")
        {
            // ��ֹ� �ı�
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
