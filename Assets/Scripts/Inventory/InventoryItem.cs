using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Item information")]
    public string thisName;
    public string thisDescription;
    public string thisFunctionality;

    [Header("Consumption")]
    public bool isConsumable;
    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;
    private GameObject itemPendingConsumption;

    [Header("Equipping")]
    public bool isEquippable;
    [HideInInspector] public bool isInsideQuickSlot;
    [HideInInspector] public bool isSelected;

    [Header("Delete items")]
    public bool isTrashable;

    [Header("Usable items")]
    public bool isUseable;

    private GameObject itemInfoUI;
    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
    }

    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
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

            if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUseable)
            {
                gameObject.SetActive(false);
                UseItem();
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
                itemInfoUI.SetActive(false);
            }
        }
    }

    private void UseItem()
    {
        itemInfoUI.SetActive(false);


        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActiveConstructionPlacement("FoundationModel");
                break;
            case "Wall(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActiveConstructionPlacement("WallModel");
                break;
            case "Floor(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActiveConstructionPlacement("FloorModel");
                break;
            case "CampFire(Clone)":
                PlacementSystem.Instance.inventoryItemToDestroy = gameObject;
                PlacementSystem.Instance.ActivatePlacementModel("CampFireModel");
                break;
            case "CampFire":
                PlacementSystem.Instance.inventoryItemToDestroy = gameObject;
                PlacementSystem.Instance.ActivatePlacementModel("CampFireModel");
                break;
            case "StorageBox(Clone)":
                PlacementSystem.Instance.inventoryItemToDestroy = gameObject;
                PlacementSystem.Instance.ActivatePlacementModel("StorageBoxModel");
                break;
            default:
                break;
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