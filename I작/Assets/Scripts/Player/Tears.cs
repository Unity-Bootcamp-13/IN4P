using UnityEngine;

public class Tears : MonoBehaviour
{
    public float projectileSpeed;
    public Transform player;    
    public Vector2 dir;

    private void Start()
    {
        projectileSpeed = player.GetComponent<Player>().projectileSpeed;
        Debug.Log(dir);
    }

    private void Update()
    {
        transform.position += (Vector3)dir * projectileSpeed * Time.deltaTime;
    }
}