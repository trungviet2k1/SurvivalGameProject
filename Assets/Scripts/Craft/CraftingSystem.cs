using System;
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
    Button craftAxeButton, craftBirchButton;

    //Requirement Items
    Text AxeReq1, AxeReq2, birchReq1;

    [HideInInspector] public bool isOpen;

    //All Blueprint
    public Blueprint AxeBLP = new("StoneAxe", 2, "Stone", 3, "Stick", 3, 1);
    public Blueprint TreatedBirchWoodBLP = new("TreatedBirchWood", 1, "Birch", 1, "", 0, 2);

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
        AxeReq1 = craftingScreenUI.transform.Find("Tools").transform.Find("ToolMenu").transform.Find("StoneAxe").transform.Find("ReqItem1").GetComponent<Text>();
        AxeReq2 = craftingScreenUI.transform.Find("Tools").transform.Find("ToolMenu").transform.Find("StoneAxe").transform.Find("ReqItem2").GetComponent<Text>();

        craftAxeButton = craftingScreenUI.transform.Find("Tools").transform.Find("ToolMenu").transform.Find("StoneAxe").transform.Find("Craft").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //TreatedBirchWood
        birchReq1 = craftingScreenUI.transform.Find("Architecture").transform.Find("ArchitectureMenu").transform.Find("TreatedBirchWood").transform.Find("ReqItem1").GetComponent<Text>();

        craftBirchButton = craftingScreenUI.transform.Find("Architecture").transform.Find("ArchitectureMenu").transform.Find("TreatedBirchWood").transform.Find("Craft").GetComponent<Button>();
        craftBirchButton.onClick.AddListener(delegate { CraftAnyItem(TreatedBirchWoodBLP); });
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
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingItemSound);
        StartCoroutine(CraftedDelayForSound(blueprintToCraft));

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.ReqAmount1);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req2, blueprintToCraft.ReqAmount2);
        }

        StartCoroutine(Calculate());
    }

    IEnumerator CraftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        for (var i = 0; i < blueprintToCraft.numOfResults; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
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
        int birchLog_count = 0;

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
                case "Birch":
                    birchLog_count += 1;
                    break;
            }
        }

        //Axe
        AxeReq1.text = "3 Stones ["+ stone_count +"/3]";
        AxeReq2.text = "3 Sticks ["+ stick_count +"/3]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        //TreatedBirchWood
        birchReq1.text = "1 Birch log [" + birchLog_count + "/1]";

        if (birchLog_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftBirchButton.gameObject.SetActive(true);
        }
        else
        {
            craftBirchButton.gameObject.SetActive(false);
        }
    }
}