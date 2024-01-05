using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    [System.Serializable]
    public class TreeData
    {
        public GameObject treeObject;
        public int treeAmount;
    }

    public List<TreeData> treesData = new List<TreeData>();
    public Terrain terrain;

    void Start()
    {
        foreach (TreeData data in treesData)
        {
            GenerateTrees(data.treeObject, data.treeAmount);
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
            newTree.transform.parent = transform; // or treeInWorldObject.transform;
        }
    }
}