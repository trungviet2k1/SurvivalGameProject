using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public List<string> inventoryItemList = new List<string>();

    //Craft Buttons
    Button craftButton;

    //Requirement Items
    Text req1, req2;

    [HideInInspector] public bool isOpen;

    //All Blueprint
    public Blueprint AxeBLP = new("StoneAxe", 2, "Stone", 3, "Stick", 3);

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
        isOpen = false;

        //Axe
        req1 = craftingScreenUI.transform.Find("StoneAxe").transform.Find("ReqItem1").GetComponent<Text>();
        req2 = craftingScreenUI.transform.Find("StoneAxe").transform.Find("ReqItem2").GetComponent<Text>();

        craftButton = craftingScreenUI.transform.Find("StoneAxe").transform.Find("Craft").GetComponent<Button>();
        craftButton.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
        }
        else if(blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req2, blueprintToCraft.ReqAmount2);
        }

        StartCoroutine(Calculate());
    }

    public IEnumerator Calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeedItems();
    }

    public void RefreshNeedItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        //Axe
        req1.text = "3 Stones ["+ stone_count +"/3]";
        req2.text = "3 Sticks ["+ stick_count +"/3]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftButton.gameObject.SetActive(true);
        }
        else
        {
            craftButton.gameObject.SetActive(false);
        }
    }
}