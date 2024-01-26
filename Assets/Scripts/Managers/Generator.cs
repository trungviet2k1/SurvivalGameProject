using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject prefab;
        public int amount;
    }

    public List<SpawnData> allSpawnData = new List<SpawnData>();

    public Terrain terrain;
    public GameObject plants;
    public GameObject animals;
    public GameObject stones;

    private Dictionary<GameObject, List<GameObject>> objectContainers = new();

    void Start()
    {
        objectContainers.Add(plants, new List<GameObject>());
        objectContainers.Add(animals, new List<GameObject>());
        objectContainers.Add(stones, new List<GameObject>());

        GenerateObjects();
    }

    void GenerateObjects()
    {
        foreach (SpawnData data in allSpawnData)
        {
            for (int i = 0; i < data.amount; i++)
            {
                Vector3 randomPos = GetRandomPosition(terrain.terrainData.size);

                GameObject newObject = Instantiate(data.prefab, randomPos, Quaternion.identity);

                GameObject container = GetContainerByPrefab(data.prefab);

                newObject.transform.parent = container.transform;
                objectContainers[container].Add(newObject);
            }
        }
    }

    GameObject GetContainerByPrefab(GameObject prefab)
    {
        if (prefab.CompareTag("Plant"))
        {
            return plants;
        }
        else if (prefab.CompareTag("Animal"))
        {
            return animals;
        }
        else if (prefab.CompareTag("PickAble"))
        {
            return stones;
        }
        else
        {
            return null;
        }
    }

    Vector3 GetRandomPosition(Vector3 terrainSize)
    {
        float randomX = Random.Range(0f, terrainSize.x);
        float randomZ = Random.Range(0f, terrainSize.z);

        Vector3 randomPos = new Vector3(randomX, 0f, randomZ);
        randomPos.y = terrain.SampleHeight(randomPos);

        return randomPos;
    }
}