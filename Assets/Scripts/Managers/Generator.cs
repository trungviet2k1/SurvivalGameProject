using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [System.Serializable]
    public class TreeData
    {
        public GameObject treeObject;
        public int treeAmount;
    }

    [System.Serializable]
    public class RabitData
    {
        public GameObject rabitObject;
        public int rabitAmount;
    }

    [System.Serializable]
    public class StoneData
    {
        public GameObject stoneObject;
        public int stoneAmount;
    }

    public List<TreeData> treesData = new List<TreeData>();
    public List<RabitData> rabitsData = new List<RabitData>();
    public List<StoneData> stonesData = new List<StoneData>();

    public Terrain terrain;
    public GameObject planes;
    public GameObject animals;
    public GameObject stones;

    void Start()
    {
        foreach (TreeData data in treesData)
        {
            GenerateTrees(data.treeObject, data.treeAmount);
        }

        foreach (RabitData data in rabitsData)
        {
            GenerateRabits(data.rabitObject, data.rabitAmount);
        }

        foreach (StoneData data in stonesData)
        {
            GenerateStones(data.stoneObject, data.stoneAmount);
        }
    }

    void GenerateTrees(GameObject treeObject, int treeAmount)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < treeAmount; i++)
        {
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            Vector3 randomPos = new Vector3(randomX, 0f, randomZ);

            // Convert position to world space
            randomPos.y = terrain.SampleHeight(randomPos);

            GameObject newTree = Instantiate(treeObject, randomPos, Quaternion.identity);
            newTree.transform.parent = planes.transform;
        }
    }

    void GenerateRabits(GameObject rabitObject, int rabitAmount)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < rabitAmount; i++)
        {
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            Vector3 randomPos = new Vector3(randomX, 0f, randomZ);

            randomPos.y = terrain.SampleHeight(randomPos);

            GameObject newRabit = Instantiate(rabitObject, randomPos, Quaternion.identity);
            newRabit.transform.parent = animals.transform;
        }
    }

    void GenerateStones(GameObject stoneObject, int stoneAmount)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < stoneAmount; i++)
        {
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            Vector3 randomPos = new Vector3(randomX, 0f, randomZ);

            randomPos.y = terrain.SampleHeight(randomPos);

            GameObject newStone = Instantiate(stoneObject, randomPos, Quaternion.identity);
            newStone.transform.parent = stones.transform;
        }
    }
}