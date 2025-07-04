using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gusher : MonoBehaviour
{
    [SerializeField] CharacterData gaperData;
    public int gusher_hp;
    public float gusher_atk;
    public float gusher_atkSpeed;
    public float gusher_speed;
    public float gusher_atkRange;
    public float gusher_projectileSpeed;
    public int gusher_currentHp;
    public float gusher_projectileDamage = 1f;

    public Animator gusher_Animator;

    public GameObject tearsPrefab;

    public Rigidbody2D gusher_rb;

    Vector2 MoveDir;
    int movDirX;
    int movDirY;

    private void Awake()
    {
        gusher_hp = gaperData.PlayerHp;
        gusher_atk = gaperData.Atk;
        gusher_atkSpeed = gaperData.AtkSpeed;
        gusher_speed = gaperData.Speed;
        gusher_atkRange = gaperData.AtkRange;
        gusher_projectileSpeed = gaperData.ProjectileSpeed;
        
        gusher_currentHp = gusher_hp;

        gusher_rb = GetComponent<Rigidbody2D>();
        gusher_Animator = GetComponent<Animator>();
        Debug.Log($"[Gusher] 셋팅된 눈물 데미지{gusher_projectileDamage})");
        


    }

    private void Start()
    {
        StartCoroutine(MoveRandom());
        StartCoroutine(tearsSpawnRoutine());
    }


    private void FixedUpdate()
    {
        gusher_rb.MovePosition(gusher_rb.position + MoveDir * gusher_speed * Time.deltaTime);
        

    }

    IEnumerator MoveRandom()
    {
        while (true)
        {
            movDirX = Random.Range(-1, 2);
            movDirY = Random.Range(-1, 2);
            
            while (movDirX == 0 && movDirY == 0)
            {
                movDirX = Random.Range(-1, 2);
                movDirY = Random.Range(-1, 2);
                
            }

            MoveDir = new Vector2(movDirX, movDirY).normalized;

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator tearsSpawnRoutine()
    {
        while(true)
        {
            tearsSpawn();
            yield return new WaitForSeconds(gusher_atkSpeed);
        }
        
    }

    void tearsSpawn()
    {
        Vector2 dir = new Vector2(movDirX, movDirY);
        float angleOffset = Random.Range(-10f, 10f);
        Quaternion rot = Quaternion.AngleAxis(angleOffset, Vector3.forward);
        Vector2 rotatedDir = rot * dir;

        GameObject tears = Instantiate(tearsPrefab,transform.position, rot);
        NormalTears et = tears.GetComponent<NormalTears>();
        Debug.Log($"[Gusher] SetTears 호출 직전 damage: {gusher_projectileDamage})");
        et.SetTears(gusher_projectileSpeed, gusher_atkRange, (int)gusher_projectileDamage,rotatedDir);

    }

    public void TakeDamage(int damage)
    {
        gusher_hp -= damage;

        if (gusher_hp <= 0)
        {
            gusher_Animator.SetTrigger("IsDead");
            this.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(DieAnimation());
        }
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                Debug.Log($"Gusher몸박데미지{gusher_atk} 입음");
                player.TakeDamage((int)gusher_atk * 2);
            }
        }
    }
}
