using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    public bool playerInRange;

    [Header("Name")]
    public string fruitName;

    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("List Fruits")]
    [SerializeField] List<GameObject> fruitsSpawn;
    [SerializeField] GameObject fruitsPrefab;

    private bool fruitsSpawned = false;

    private void Start()
    {
        SpawnFruitsOnce();
    }

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        PickFruits();
    }

    private void SpawnFruitsOnce()
    {
        if (!fruitsSpawned)
        {
            foreach (GameObject spawnPoint in fruitsSpawn)
            {
                GameObject fruit = Instantiate(fruitsPrefab, spawnPoint.transform.position, Quaternion.identity);

                fruit.transform.parent = spawnPoint.transform;

                Vector3 fruitPosition = Vector3.zero;
                fruitPosition.y = 0f;
                fruit.transform.localPosition = fruitPosition;

                fruitsSpawned = true;
            }
        }
    }

    private void PickFruits()
    {
        bool noFruitsRemaining = true;

        foreach (GameObject spawnPoint in fruitsSpawn)
        {
            if (spawnPoint.transform.childCount > 0)
            {
                noFruitsRemaining = false;
                break;
            }
        }

        if (noFruitsRemaining)
        {
            StartCoroutine(PlantDead());
        }
    }

    IEnumerator PlantDead()
    {
        yield return new WaitForSeconds(4f);

        int numberOfSeeds = Random.Range(1, 3);

        GameObject items = GetComponentInParent<EnvironmentManager>().allFruits;

        for (int i = 0; i < numberOfSeeds; i++)
        {
            float yOffset = i * 0.2f;
            GameObject stoneModel = Instantiate(Resources.Load("Tomato Seed_Model"),
                transform.position + Vector3.up * yOffset, Quaternion.identity) as GameObject;
            stoneModel.transform.parent = items.transform;
        }

        Destroy(transform.gameObject);
    }
}