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

    public List<SpawnData> allSpawnData = new();

    public List<Terrain> terrains;
    public GameObject plants;
    public GameObject animals;
    public GameObject items;

    private readonly Dictionary<GameObject, List<GameObject>> objectContainers = new();

    void Start()
    {
        objectContainers.Add(plants, new List<GameObject>());
        objectContainers.Add(animals, new List<GameObject>());
        objectContainers.Add(items, new List<GameObject>());

        GenerateObjectsOnTerrains();
    }

    void GenerateObjectsOnTerrains()
    {
        foreach (Terrain terrain in terrains)
        {
            GenerateObjectsOnTerrain(terrain);
        }
    }

    void GenerateObjectsOnTerrain(Terrain terrain)
    {
        foreach (SpawnData data in allSpawnData)
        {
            for (int i = 0; i < data.amount; i++)
            {
                Vector3 randomPos = GetRandomPosition(terrain.terrainData.size, terrain.GetPosition());

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
            return items;
        }
        else
        {
            return null;
        }
    }

    Vector3 GetRandomPosition(Vector3 terrainSize, Vector3 terrainPosition)
    {
        float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
        float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

        Vector3 randomPos = new(randomX, 0f, randomZ);
        randomPos.y = terrains[0].SampleHeight(randomPos);
        randomPos.y = terrains[1].SampleHeight(randomPos);

        return randomPos;
    }
}