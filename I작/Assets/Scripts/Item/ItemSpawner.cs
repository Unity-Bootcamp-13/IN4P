using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemRepositorySO itemRepositorySO;
    public GameObject testItem;
    List<int> itemIds;

    private void Start()
    {
        itemIds = itemRepositorySO.itemRepository.GetAllId();
    }

    void Update()
    {    
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 randomPosition = Random.insideUnitCircle;
            var go = Instantiate(testItem, transform.position + randomPosition * 3f, Quaternion.identity);

            int random = Random.Range(0, itemIds.Count);
            go.GetComponent<Item>().itemId = random;
            Debug.Log(random);
        }
    }
}
