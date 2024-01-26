using UnityEngine;

public class EnvironmentGeneratorManager : MonoBehaviour
{
    public static EnvironmentGeneratorManager Instance { get; set; }

    public GameObject allTrees;
    public GameObject allAnimals;
    public GameObject allStones;

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
