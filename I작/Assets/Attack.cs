using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject tearsPrefab;

    bool launchPlace;

    public Transform leftEye;
    public Transform rightEye;

    void CreateTears(string dir)
    {
        Debug.Log("´«¹°");
        GameObject go = Instantiate(tearsPrefab);

        switch (dir)
        {
            case "Up":
                go.GetComponent<Tears>().dir = new Vector2(0, 1);
                break;
            case "Down":
                go.GetComponent<Tears>().dir = new Vector2(0, -1);
                break;
            case "Left":
                go.GetComponent<Tears>().dir = new Vector2(-1, 0);
                break;
            case "Right":
                go.GetComponent<Tears>().dir = new Vector2(1, 0);
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
    }
}
