using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    public PlayerBodyMovement bodyMovement;
    public PlayerHeadMovement headMovement;
    //[SerializeField] private GameObject headObject;
    //[SerializeField] private GameObject bodyObject;

    private void Start()
    {
        //headObject = GetComponent<GameObject>();
        //bodyObject = GetComponent<GameObject>();
    }

    private Vector2 last = Vector2.down;


    public void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector2(h, v);
        transform.position += dir * stat.speed * Time.deltaTime;
        bodyMovement.SetBodyAnimation();
        headMovement.SetHeadAnimation();
        

    }
}
