using System;
using System.Collections;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; set; }

    public bool inPlacementMode;
    public bool isValidPlacement;

    [Header("Placement Holding Spawn")]
    public GameObject placementHoldingSpot;

    [Header("Environment Placeables")]
    public GameObject environmentPlaceables;

    [Header("Items Placed")]
    public GameObject itemToBePlaced;
    public GameObject inventoryItemToDestroy;

    [Header("Mode UI")]
    public GameObject placementModeUI;


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

    public void ActivatePlacementModel(string itemToPlace)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToPlace));
        item.name = itemToPlace;
        item.transform.SetParent(placementHoldingSpot.transform, false);
        itemToBePlaced = item;
        inPlacementMode = true;
    }

    void Update()
    {
        if (inPlacementMode)
        {
            placementModeUI.SetActive(true);
        }
        else
        {
            placementModeUI.SetActive(false);
        }

        if (itemToBePlaced != null && inPlacementMode)
        {
            if (IsCheckValidPlacement())
            {
                isValidPlacement = true;
                itemToBePlaced.GetComponent<PlaceableItem>().SetValidColor();
            }
            else
            {
                isValidPlacement = false;
                itemToBePlaced.GetComponent<PlaceableItem>().SetInvalidColor();
            }
        }

        if (Input.GetMouseButtonDown(0) && inPlacementMode && isValidPlacement)
        {
            PlaceItemFreeStyle();
            DestroyItem(inventoryItemToDestroy);
        }

        if (Input.GetKeyDown(KeyCode.X) && inPlacementMode)
        {
            inventoryItemToDestroy.SetActive(true);
            inventoryItemToDestroy = null;
            DestroyItem(itemToBePlaced);
            itemToBePlaced = null;
            inPlacementMode = false;
        }
    }

    private bool IsCheckValidPlacement()
    {
        if (itemToBePlaced != null)
        {
            return itemToBePlaced.GetComponent<PlaceableItem>().isValidToBeBuild;
        }

        return false;
    }

    private void PlaceItemFreeStyle()
    {
        itemToBePlaced.transform.SetParent(environmentPlaceables.transform, true);

        itemToBePlaced.GetComponent<PlaceableItem>().SetDefaultColor();
        itemToBePlaced.GetComponent<PlaceableItem>().enabled = false;

        itemToBePlaced = null;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        inPlacementMode = false;
    }

    private void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();
    }
}
