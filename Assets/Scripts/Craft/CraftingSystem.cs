using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance { get; set; }

    public GameObject craftingScreenUI;
    public List<string> inventoryItemList = new List<string>();

    //Craft Buttons
    Button craftButton;

    //Requirement Items
    Text req1, req2;

    [HideInInspector] public bool isOpen;

    //All Blueprint
    public Blueprint AxeBLP = new("Axe", 2, "Stone", 3, "Stick", 3);

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        isOpen = false;

        //Axe
        req1 = craftingScreenUI.transform.Find("Axe").transform.Find("ReqItem1").GetComponent<Text>();
        req2 = craftingScreenUI.transform.Find("Axe").transform.Find("ReqItem2").GetComponent<Text>();

        craftButton = craftingScreenUI.transform.Find("Axe").transform.Find("Craft").GetComponent<Button>();
        craftButton.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    void Update()
    {
        RefreshNeedItems();

        if (Input.GetKeyDown(KeyCode.B) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            if (!InventorySystem.instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        InventorySystem.instance.AddToInventory(blueprintToCraft.itemName);

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
        }
        else if(blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
            InventorySystem.instance.RemoveItem(blueprintToCraft.req2, blueprintToCraft.ReqAmount2);
        }

        StartCoroutine(Calculate());
        RefreshNeedItems();
    }

    public IEnumerator Calculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.instance.ReCalculateList();
    }

    private void RefreshNeedItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.instance.itemList;

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