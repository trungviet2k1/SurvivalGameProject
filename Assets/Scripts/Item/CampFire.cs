using System;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public bool playerInRange;
    public bool isCooking;
    public float cookingTimer;

    public CookableFoods foodBeingCooked;
    public string readyFood;

    public GameObject fire;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }

        if (cookingTimer <= 0 && isCooking)
        {
            isCooking = false;
            readyFood = GetCookedFood(foodBeingCooked);
        }
    }

    private string GetCookedFood(CookableFoods food)
    {
        return food.cookedFoodName;
    }

    public void OpenUI()
    {
        CampFireUIManager.Instance.OpenUI();
        CampFireUIManager.Instance.selectedCampfire = this;

        if (readyFood != "")
        {
            GameObject rf = Instantiate(Resources.Load<GameObject>(readyFood),
                CampFireUIManager.Instance.foodSlot.transform.position,
                CampFireUIManager.Instance.foodSlot.transform.rotation);

            rf.transform.SetParent(CampFireUIManager.Instance.foodSlot.transform);

            readyFood = "";
        }
    }

    public void StartCooking(InventoryItem food)
    {
        foodBeingCooked = ConverIntoCookable(food);

        isCooking = true;

        cookingTimer = TimeToCookFood(foodBeingCooked);
    }

    private CookableFoods ConverIntoCookable(InventoryItem food)
    {
        foreach (CookableFoods cookable in CampFireUIManager.Instance.cookingData.validFoods)
        {
            if (cookable.name == food.thisName)
            {
                return cookable;
            }
        }
        return new CookableFoods();
    }

    private float TimeToCookFood(CookableFoods food)
    {
        return food.timeToCook;
    }
}