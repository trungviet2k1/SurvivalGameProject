using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    [Header("UI")]
    public GameObject craftingScreenUI;

    [Header("Item in Inventory")]
    public List<string> inventoryItemList = new List<string>();

    [Header("Set up crafting items")]
    public Blueprint[] blueprints;
    public Text[] requiredItems;
    public Button[] craftedButton;

    [HideInInspector] public bool isOpen;

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

        for (int i = 0; i < blueprints.Length; i++)
        {
            int index = i;
            craftedButton[index].onClick.AddListener(delegate { CraftAnyItem(blueprints[index]); });
        }

        RefreshNeedItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isOpen && !ConstructionManager.Instance.inConstrucionMode)
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

        for (int i = 0; i < blueprintToCraft.numOfRequirements; i++)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.requiredItems[i], blueprintToCraft.numberOfRequests[i]);
        }

        StartCoroutine(Calculate());
    }

    IEnumerator CraftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < blueprintToCraft.numberOfItemsCreated; i++)
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
        int ingotIron_count = 0;
        int log_count = 0;
        int treatedWood_count = 0;
        int plank_cound = 0;

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
                case "Ingot Iron":
                    ingotIron_count += 1;
                    break;
                case "Log":
                    log_count += 1;
                    break;
                case "TreatedWood":
                    treatedWood_count += 1;
                    break;
                case "Plank":
                    plank_cound += 1;
                    break;
            }
        }

        //Axe
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[0] != null && requiredItems[0] != null && requiredItems[1] != null)
            {
                Transform axeReq1 = requiredItems[0].transform;
                Transform axeReq2 = requiredItems[1].transform;
                axeReq1.GetComponent<Text>().text = "3 Stones [" + stone_count + "/3]";
                axeReq2.GetComponent<Text>().text = "3 Sticks [" + stick_count + "/3]";

                if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[0].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[0].gameObject.SetActive(false);
                }
            }
        }

        //TreatedWood
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[1] != null && requiredItems[2] != null)
            {
                Transform birchReq = requiredItems[2].transform;
                birchReq.GetComponent<Text>().text = "1 Log [" + log_count + "/1]";

                if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
                {
                    craftedButton[1].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[1].gameObject.SetActive(false);
                }
            }
        }

        //Plank
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[2] != null && requiredItems[3] != null)
            {
                Transform plankReq = requiredItems[3].transform;
                plankReq.GetComponent<Text>().text = "1 Treated wood [" + treatedWood_count + "/1]";

                if (treatedWood_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(4))
                {
                    craftedButton[2].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[2].gameObject.SetActive(false);
                }
            }
        }

        //Foundation
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[3] != null && requiredItems[4] != null)
            {
                Transform foundationReq = requiredItems[4].transform;
                foundationReq.GetComponent<Text>().text = "4 Planks [" + plank_cound + "/4]";

                if (plank_cound >= 4 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[3].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[3].gameObject.SetActive(false);
                }
            }
        }

        //Wall
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[4] != null && requiredItems[5] != null)
            {
                Transform wallReq = requiredItems[5].transform;
                wallReq.GetComponent<Text>().text = "2 Planks [" + plank_cound + "/2]";

                if (plank_cound >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[4].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[4].gameObject.SetActive(false);
                }
            }
        }

        //Floor
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[5] != null && requiredItems[6] != null)
            {
                Transform floorReq = requiredItems[6].transform;
                floorReq.GetComponent<Text>().text = "2 Planks [" + plank_cound + "/2]";

                if (plank_cound >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[5].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[5].gameObject.SetActive(false);
                }
            }
        }

        //Storage
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[6] != null && requiredItems[7] != null)
            {
                Transform storageReq = requiredItems[7].transform;
                storageReq.GetComponent<Text>().text = "8 Planks [" + plank_cound + "/8]";

                if (plank_cound >= 8 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[6].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[6].gameObject.SetActive(false);
                }
            }
        }

        //CampFire
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[7] != null && requiredItems[8] != null && requiredItems[9] != null)
            {
                Transform campfire1Req = requiredItems[8].transform;
                Transform campfire2Req = requiredItems[9].transform;
                campfire1Req.GetComponent<Text>().text = "8 Stone [" + stone_count + "/8]";
                campfire2Req.GetComponent<Text>().text = "8 Stick [" + stick_count + "/8]";

                if (stone_count >= 8 && stick_count >= 8 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[7].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[7].gameObject.SetActive(false);
                }
            }
        }

        //Pick Axe
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[8] != null && requiredItems[10] != null && requiredItems[11] != null)
            {
                Transform pickAxeReq1 = requiredItems[10].transform;
                Transform pickAxeReq2 = requiredItems[11].transform;
                pickAxeReq1.GetComponent<Text>().text = "6 Stones [" + stone_count + "/6]";
                pickAxeReq2.GetComponent<Text>().text = "3 Sticks [" + stick_count + "/3]";

                if (stone_count >= 6 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[8].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[8].gameObject.SetActive(false);
                }
            }
        }

        //Furnace
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[9] != null && requiredItems[12] != null)
            {
                Transform furnaceReq1 = requiredItems[12].transform;
                furnaceReq1.GetComponent<Text>().text = "8 Stones [" + stone_count + "/8]";

                if (stone_count >= 8 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[9].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[9].gameObject.SetActive(false);
                }
            }
        }

        //IronAxe
        if (craftedButton.Length > 0 && requiredItems.Length > 0)
        {
            if (craftedButton[10] != null && requiredItems[13] != null && requiredItems[14] != null)
            {
                Transform ironAxeReq1 = requiredItems[13].transform;
                Transform ironAxeReq2 = requiredItems[14].transform;
                ironAxeReq1.GetComponent<Text>().text = "3 Ingot Iron [" + ingotIron_count + "/3]";
                ironAxeReq2.GetComponent<Text>().text = "3 Sticks [" + stick_count + "/3]";

                if (ingotIron_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    craftedButton[10].gameObject.SetActive(true);
                }
                else
                {
                    craftedButton[10].gameObject.SetActive(false);
                }
            }
        }
    }
}