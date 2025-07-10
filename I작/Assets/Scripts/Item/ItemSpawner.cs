using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemServiceSO itemServiceSO;
    private ItemGenerator itemGenerator;


    private void Start()
    {
        itemGenerator = new ItemGenerator(itemServiceSO.itemService);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            var go = itemGenerator.GetRandomPickupItem(transform.position + randomPosition);
            go.transform.position = transform.position + randomPosition * 3f;

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            var go = itemGenerator.GetRandomPassiveItem(transform.position + randomPosition);
            go.transform.position = transform.position + randomPosition * 3f;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            var go = itemGenerator.GetRandomActiveItem(transform.position + randomPosition);
            go.transform.position = transform.position + randomPosition * 3f;
        }
    }
}
