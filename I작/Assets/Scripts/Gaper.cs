using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Gaper : MonoBehaviour
{
    [SerializeField] CharacterData GaperData;
    public int gaper_hp;
    public float gaper_atk;
    public float gaper_atkSpeed;
    public float gaper_speed;
    public float gaper_atkRange;
    public float gaper_projectileSpeed;
    public int gaper_currentHp;

    public Animator gaperAnimator;

    public GameObject target;
    public Rigidbody2D gaper_rb;
    public Collider2D gaperCollider;

    public GameObject GusherPrefab;

    private void Awake()
    {
        gaper_hp = GaperData.PlayerHp;
        gaper_atk = GaperData.Atk;
        gaper_atkSpeed = GaperData.AtkSpeed;
        gaper_speed = GaperData.Speed;
        gaper_atkRange = GaperData.AtkRange;
        gaper_currentHp = gaper_hp;

        target = GameObject.FindGameObjectWithTag("Player");
        gaperAnimator = transform.GetChild(0).GetComponent<Animator>();
        gaper_rb = GetComponent<Rigidbody2D>();
        gaperCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }


    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        ChasePlayerMovement();
    }
    public void ChasePlayerMovement()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);

        if (distance > gaper_atkRange)
        {
            Vector2 direciton = (target.transform.position - transform.position).normalized;
            gaper_rb.MovePosition(gaper_rb.position + direciton * gaper_speed * Time.deltaTime);
        }

    }

    public void TakeDamage(int damage)
    {
        gaper_hp -= damage;

        if (gaper_hp <= 0)
        {
            
            gaperCollider.enabled = false;
            
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand > 0)
        {
            StartCoroutine(SpawnGusher());
        }
        this.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator SpawnGusher()
    {
        Instantiate(GusherPrefab, transform.position, quaternion.identity);
        yield return null;
    }
}
