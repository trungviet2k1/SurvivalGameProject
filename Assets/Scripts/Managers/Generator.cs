using System.Collections;
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
    public GameObject parent;

    void Start()
    {
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
                newObject.transform.parent = parent.transform;
            }
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