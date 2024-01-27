using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CampFireUIManager : MonoBehaviour
{
    public static CampFireUIManager Instance { get; set; }

    [Header("Camp Fire UI")]
    public GameObject campfirePanel;
    public GameObject fuelSlot;
    public GameObject foodSlot;
    public Button cookButton;
    public Button exitButton;

    [Header("Data")]
    public CookingData cookingData;

    [HideInInspector] public bool isUIOpen;
    [HideInInspector] public CampFire selectedCampfire;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (FuelAndFoodAreValid())
        {
            cookButton.interactable = true;
        }
        else
        {
            cookButton.interactable = false;
        }
    }

    private bool FuelAndFoodAreValid()
    {
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();

        if (fuel != null && food != null)
        {
            if (cookingData.validFuels.Contains(fuel.thisName) 
                && cookingData.validFoods.Any(cookableFoods => cookableFoods.name == food.thisName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public void CookButtonPressed()
    {
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();
        selectedCampfire.StartCooking(food);
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        Destroy(fuel.gameObject);
        Destroy(food.gameObject);

        CloseUI();
    }

    public void OpenUI()
    {
        campfirePanel.SetActive(true);
        isUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        InventorySystem.Instance.OpenUI();
    }

    public void CloseUI()
    {
        campfirePanel.SetActive(false);
        isUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }
}
