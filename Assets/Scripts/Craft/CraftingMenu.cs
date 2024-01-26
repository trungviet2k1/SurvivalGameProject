using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    [System.Serializable]
    public struct CraftingItemInfo
    {
        public GameObject menuObject;
        public CanvasGroup canvasGroup;
        public GameObject craftObject;
    }

    public static CraftingMenu Instance { get; set; }

    [Header("Craftable items")]
    public List<CraftingItemInfo> craftingItems;

    [Header("Menu Craft")]
    public GameObject menuToolCraft;
    public GameObject menuResourcesCraft;
    public GameObject menuConstructionsCraft;

    private void Awake()
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
        foreach (var item in craftingItems)
        {
            item.menuObject.SetActive(false);
            item.canvasGroup.alpha = 0.5f;
            item.craftObject.SetActive(false);
        }

        menuToolCraft.SetActive(false);
        menuResourcesCraft.SetActive(false);
        menuConstructionsCraft.SetActive(false);
    }

    public void OnCraftingItemClick(int index)
    {
        for (int i = 0; i < craftingItems.Count; i++)
        {
            if (i == index)
            {
                craftingItems[i].canvasGroup.alpha = 1f;
                craftingItems[i].craftObject.SetActive(true);
                ShowMenuCraft(index);
            }
            else
            {
                craftingItems[i].canvasGroup.alpha = 0.5f;
                craftingItems[i].craftObject.SetActive(false);
            }
        }
    }

    private void ShowMenuCraft(int index)
    {
        menuToolCraft.SetActive(false);
        menuResourcesCraft.SetActive(false);
        menuConstructionsCraft.SetActive(false);

        switch (index)
        {
            case 0:
                menuToolCraft.SetActive(true);
                craftingItems[0].craftObject.SetActive(true);
                break;
            case 1:
                menuToolCraft.SetActive(true);
                craftingItems[1].craftObject.SetActive(true);
                break;
            case 2:
                menuResourcesCraft.SetActive(true);
                craftingItems[2].craftObject.SetActive(true);
                break;
            case 3:
                menuResourcesCraft.SetActive(true);
                craftingItems[3].craftObject.SetActive(true);
                break;
            case 4:
                menuConstructionsCraft.SetActive(true);
                craftingItems[4].craftObject.SetActive(true);
                break;
            case 5:
                menuConstructionsCraft.SetActive(true);
                craftingItems[5].craftObject.SetActive(true);
                break;
            case 6:
                menuConstructionsCraft.SetActive(true);
                craftingItems[6].craftObject.SetActive(true);
                break;
        }
    }

    public void OnCraftingButtonClick(CanvasGroup canvasGroup, GameObject menuObject, GameObject craftObject)
    {
        foreach (var item in craftingItems)
        {
            item.canvasGroup.alpha = (item.canvasGroup == canvasGroup) ? 1f : 0.5f;
            item.menuObject.SetActive(false);
            item.craftObject.SetActive(false);
        }

        canvasGroup.alpha = 1f;
        menuObject.SetActive(true);

        menuToolCraft.SetActive(false);
        menuResourcesCraft.SetActive(false);
        menuConstructionsCraft.SetActive(false);

        if (craftObject != null)
        {
            craftObject.SetActive(true);
        }
    }
}