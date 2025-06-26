using UnityEngine;

public class Tears : MonoBehaviour
{
    public float speed;
    public Player player;
    public Vector3 dir;

    private void Start()
    {
        speed = player.GetComponent<Player>().projectileSpeed;
    }

    private void Update()
    {
        transform.position += dir*speed*Time.deltaTime;
    }
}