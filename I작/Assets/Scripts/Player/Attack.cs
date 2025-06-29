using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject tearsPrefab;
    // poolSize 이상으로 눈물이 만들어지면
    // 새로 만들어진 눈물의 사거리와 속도가 초기화가 안되는 문제가 있음
    [SerializeField] private int poolSize = 50;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform rightEye;

    private bool launchPlace;

    int tearsCount = 1;
    private bool isHoming;
    private bool isPierce;


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

    public void UpdateTears(float tearSpeed, float atkRange)
    {
        foreach (GameObject tear in pool)
        {
            tear.GetComponent<Tears>().SetTears(tearSpeed, atkRange);
        }
    }
}
