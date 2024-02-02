using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceUIManager : MonoBehaviour
{
    public static FurnaceUIManager Instance { get; set; }

    [Header("Furnace UI")]
    public GameObject furnacePanel;
    public GameObject fuelSlot;
    public GameObject mineralSlot;
    public Button calcinedButton;
    public Button exitButton;

    [Header("Data")]
    public FurnacingData furnacingData;

    [HideInInspector] public bool isUIOpen;
    [HideInInspector] public Furnace selectedFurnace;

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
        if (FuelAndMineralAreValid())
        {
            calcinedButton.interactable = true;
        }
        else
        {
            calcinedButton.interactable = false;
        }
    }

    private bool FuelAndMineralAreValid()
    {
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem mineral = mineralSlot.GetComponentInChildren<InventoryItem>();

        if (fuel != null && mineral != null)
        {
            if (furnacingData.validFuels.Contains(fuel.thisName)
                && furnacingData.validMinerals.Any(calcinableMinerals => calcinableMinerals.name == mineral.thisName))
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

    public void CalcinableButtonPressed()
    {
        InventoryItem mineral = mineralSlot.GetComponentInChildren<InventoryItem>();
        selectedFurnace.StartFurnacing(mineral);
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        Destroy(fuel.gameObject);
        Destroy(mineral.gameObject);

        CloseUI();
    }

    public void OpenUI()
    {
        furnacePanel.SetActive(true);
        isUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        InventorySystem.Instance.OpenUI();
    }

    public void CloseUI()
    {
        furnacePanel.SetActive(false);
        isUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }
}
