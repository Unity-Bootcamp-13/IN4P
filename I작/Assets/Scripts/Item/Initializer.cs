using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private ItemServiceSO itemserviceSO;

    private void Awake()
    {
        itemserviceSO.Init();
    }
}