using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool isTrashable;

    private GameObject itemInfoUI;

    //Item Info UI
    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    //Consumption
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;

    void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                ConsumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeedItems();
            }
        }
    }

    private void ConsumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(true);
        HealthEffectCalculation(healthEffect);
        CaloriesEffectCalculation(caloriesEffect);
        HydrationEffectCalculation(hydrationEffect);
    }

    private void HealthEffectCalculation(float healthEffect)
    {
        //Health;
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if ((healthBeforeConsumption + healthEffect) > maxHealth)
        {
            PlayerState.Instance.SetHealth(maxHealth);
        }
        else
        {
            PlayerState.Instance.SetHealth(healthBeforeConsumption + healthEffect);
        }
    }

    private void CaloriesEffectCalculation(float caloriesEffect)
    {
        //Calories;
        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + healthEffect) > maxCalories)
            {
                PlayerState.Instance.SetCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.SetCalories(caloriesBeforeConsumption + caloriesEffect);
            } 
        }
    }

    private void HydrationEffectCalculation(float hydrationEffect)
    {
        //Hydration;
        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;

        if (caloriesEffect != 0)
        {
            if ((hydrationBeforeConsumption + healthEffect) > maxHydration)
            {
                PlayerState.Instance.SetHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.SetHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }
}
