using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject tearsPrefab;
    [SerializeField] private int poolSize = 50;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform rightEye;

    private bool launchPlace;


    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tear = Instantiate(tearsPrefab);
            tear.SetActive(false);
            pool.Enqueue(tear);
        }
    }

    void CreateTears(string dir)
    {
        GameObject go = GetTearFromPool();
        Tears tear = go.GetComponent<Tears>();

        switch (dir)
        {
            case "Up":
                tear.dir = new Vector2(0, 1);
                break;
            case "Down":
                tear.dir = new Vector2(0, -1);
                break;
            case "Left":
                tear.dir = new Vector2(-1, 0);
                break;
            case "Right":
                tear.dir = new Vector2(1, 0);
                break;
        }

        if (launchPlace)
        {
            go.transform.position = leftEye.position;
            launchPlace = false;
        }
        else
        {
            go.transform.position = rightEye.position;
            launchPlace = true;
        }

        tear.SetReturnAction(() => ReturnToPool(go));
    }

    private GameObject GetTearFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject tear = pool.Dequeue();
            tear.SetActive(true);
            return tear;
        }
        else
        {
            GameObject tear = Instantiate(tearsPrefab);            
            return tear;
        }
    }

    private void ReturnToPool(GameObject tear)
    {
        tear.SetActive(false);
        pool.Enqueue(tear);
    }

    public void SetTears(float speed, float range)
    {
        foreach (var tear in pool)
        {
            tear.GetComponent<Tears>().speed = speed;
            tear.GetComponent<Tears>().range = range;
        }
    }
}
