using UnityEngine;

public class RepositoryInitializer : MonoBehaviour
{
    [SerializeField] private ItemRepositorySO itemRepositorySO;

    void Awake()
    {
        itemRepositorySO.Init();
    }
}
