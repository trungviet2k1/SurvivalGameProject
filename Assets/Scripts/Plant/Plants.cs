using System;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    [Header("Plant")]
    [SerializeField] GameObject seedModel;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlantModel;

    [Header("Plants produces")]
    [SerializeField] List<GameObject> plantProduceSpawn;
    [SerializeField] GameObject producePrefab;

    [Header("Set development value")]
    public int dayOfPlanting;
    [SerializeField] int plantAge = 0;
    [SerializeField] int ageForYoungModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;
    [SerializeField] int dayForNewProduce;
    [SerializeField] int dayRemainingForNewProduceCounter;
    [SerializeField] bool isOneTimeHarvest;
    public bool isWatered;

    private void OnEnable()
    {
        TimeManager.Instance.OnDayPass.AddListener(DayPass);
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDayPass.RemoveListener(DayPass);
    }

    private void OnDestroy()
    {
        GetComponentInParent<Soil>().isEmpty = true;
        GetComponentInParent<Soil>().plantName = "";
        GetComponentInParent<Soil>().currentPlant = null;
    }

    private void DayPass()
    {
        if (isWatered)
        {
            plantAge++;
            isWatered = false;
            GetComponentInParent<Soil>().MakeSoilNotWatered();
        }

        CheckGrowth();

        if (!isOneTimeHarvest)
        {
            CheckProduce();
        }
    }

    private void CheckGrowth()
    {
        seedModel.SetActive(plantAge < ageForYoungModel);
        youngPlantModel.SetActive(plantAge >= ageForYoungModel && plantAge < ageForMatureModel);
        maturePlantModel.SetActive(plantAge >= ageForMatureModel);

        if (plantAge >= ageForMatureModel && isOneTimeHarvest)
        {
            MakePlantPickable();
        }
    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
    }

    private void CheckProduce()
    {
        if (plantAge == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawns();
        }

        if (plantAge > ageForFirstProduceBatch)
        {
            if (dayRemainingForNewProduceCounter == 0)
            {
                GenerateProduceForEmptySpawns();

                dayRemainingForNewProduceCounter = dayForNewProduce; 
            }
            else
            {
                dayRemainingForNewProduceCounter--;
            }
        }
    }

    private void GenerateProduceForEmptySpawns()
    {
        foreach (GameObject spawn in plantProduceSpawn)
        {
            if (spawn.transform.childCount == 0)
            {
                GameObject produce = Instantiate(producePrefab);

                produce.transform.parent = spawn.transform;

                Vector3 producePosition = Vector3.zero;
                producePosition.y = 0f;
                produce.transform.localPosition = producePosition;
            }
        }
    }
}