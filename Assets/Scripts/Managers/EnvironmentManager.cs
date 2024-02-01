using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; set; }

    public GameObject allItems;
    public GameObject allTrees;
    public GameObject allBushs;
    public GameObject allFruits;
    public GameObject allAnimals;
    public GameObject allRocks;
    public GameObject placeable;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
