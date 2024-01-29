using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    [Header("Quick Slot UI")]
    public GameObject quickSlotsPanel;

    [Header("Quick Slot List")]
    public List<GameObject> quickSlotsList = new();
    public GameObject numbersHolder;
    public int selectedNumber = -1;
    public GameObject selectedItem;

    [Header("Item being held")]
    public GameObject ToolHolder;
    public GameObject selectedItemModel;

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

    void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelecQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelecQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelecQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelecQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelecQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelecQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelecQuickSlot(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelecQuickSlot(8);
        }
    }

    void SelecQuickSlot(int number)
    {
        if (CheckIfSlotFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                //Change color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }

                Text toBeChanged = numbersHolder.transform.Find("Number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.white;
            }
            else
            {
                selectedNumber = -1;

                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                //Change color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(CalculateItemModel(selectedItemName)));

        selectedItemModel.transform.SetParent(ToolHolder.transform, false);
    }

    private string CalculateItemModel(string selectedItemName)
    {
        return selectedItemName switch
        {
            "StoneAxe" => "StoneAxe_Model",
            "TomatoSeed" => "Hand_Model",
            "PumpkinSeed" => "Hand_Model",
            "WateringCan" => "WateringCan_Model",
            _ => null,
        };
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool CheckIfSlotFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        };
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();

        itemToEquip.transform.SetParent(availableSlot.transform, false);
        InventorySystem.Instance.ReCalculateList();
    }

    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsPlayerHoldingSeed()
    {
        if (selectedItemModel != null)
        {
            return selectedItemModel.name switch
            {
                "Hand_Model(Clone)" => true,
                "Hand_Model" => true,
                _ => false,
            };
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }

    internal bool IsThereASwingLock()
    {
        if (selectedItemModel && selectedItemModel.GetComponent<EquipableItem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHoldingWateringCan()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<InventoryItem>().thisName switch
            {
                "Watering Can" => true,
                _ => false,
            };
        }
        else
        {
            return false;
        }
    }
}