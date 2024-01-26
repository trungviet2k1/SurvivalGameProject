using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; set; }

    [Header("UI")]
    public GameObject constructionUI;

    [Header("Player")]
    public GameObject player;
    public GameObject constructionHoldingSpot;

    [Header("Materials")]
    public Material ghostSelectedMaterial;
    public Material ghostSemiTransparentMaterial;
    public Material ghostFullTransparentMaterial;

    [Header("Construction Settings")]
    public List<GameObject> allGhostInExistence = new List<GameObject>();
    public GameObject itemToBeConstructed;
    public bool inConstrucionMode = false;
    public bool isValidPlacement;
    public bool selectingAGhost;
    public GameObject selectedGhost;
    public GameObject itemToBeDestroyed;

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

    public void ActiveConstructionPlacement(string itemToConstruct)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));

        item.name = itemToConstruct;

        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "ActiveConstructable";

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;

        inConstrucionMode = true;
    }

    private void GetAllGhosts(GameObject itemToBeConstructed)
    {
        List<GameObject> ghostList = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

        foreach (GameObject ghost in ghostList)
        {
            allGhostInExistence.Add(ghost);
        }
    }

    private void PerformGhostDeletionScan()
    {
        foreach (GameObject ghost in allGhostInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false)
                {
                    foreach (GameObject ghostX in allGhostInExistence)
                    {
                        if (ghost.gameObject != ghostX.gameObject)
                        {
                            if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                            {
                                if (ghost != null && ghostX != null)
                                {
                                    ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (GameObject ghost in allGhostInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition)
                {
                    DestroyImmediate(ghost);
                }
            }
        }
    }

    private float PositionToAccurateFloat(GameObject ghost, Func<Vector3, float> positionSelector)
    {
        if (ghost != null)
        {
            Vector3 targetPosition = ghost.transform.position;
            float pos = positionSelector(targetPosition);
            return Mathf.Round(pos * 100f) / 100f;
        }
        return 0;
    }

    private float XPositionToAccurateFloat(GameObject ghost)
    {
        return PositionToAccurateFloat(ghost, pos => pos.x);
    }

    private float ZPositionToAccurateFloat(GameObject ghost)
    {
        return PositionToAccurateFloat(ghost, pos => pos.z);
    }

    private void Update()
    {
        if (inConstrucionMode)
        {
            constructionUI.SetActive(true);
        }
        else
        {
            constructionUI.SetActive(false);
        }

        if (itemToBeConstructed != null && inConstrucionMode)
        {
            if (itemToBeConstructed.name == "FoundationModel")
            {
                if (CheckValidConstructionPosition())
                {
                    isValidPlacement = true;
                    itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                }
                else
                {
                    isValidPlacement = false;
                    itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var selectionTranform = hit.transform;
                if (selectionTranform.gameObject.CompareTag("Ghost") && itemToBeConstructed.name == "FoundationModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTranform.gameObject;
                }
                else if (selectionTranform.gameObject.CompareTag("WallGhost") && itemToBeConstructed.name == "WallModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTranform.gameObject;
                }
                else if (selectionTranform.gameObject.CompareTag("Ghost") && itemToBeConstructed.name == "FloorModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTranform.gameObject;
                }
                else
                {
                    itemToBeConstructed.SetActive(true);
                    selectedGhost = null;
                    selectingAGhost = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && inConstrucionMode)
        {
            if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "FoundationModel")
            {
                PlaceItemFreeStyle();
                DestroyItem(itemToBeDestroyed);
            }

            if (selectingAGhost)
            {
                PlaceItemInGhostPosition(selectedGhost);
                DestroyItem(itemToBeDestroyed);
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && inConstrucionMode)
        {
            itemToBeDestroyed.SetActive(true);
            itemToBeDestroyed = null;
            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            inConstrucionMode = false;
        }
    }

    private void PlaceItemInGhostPosition(GameObject copyOfGhost)
    {
        Vector3 ghostPosition = copyOfGhost.transform.position;
        Quaternion ghostRotation = copyOfGhost.transform.rotation;

        selectedGhost.gameObject.SetActive(false);

        itemToBeConstructed.gameObject.SetActive(true);
        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

        var randomOffset = UnityEngine.Random.Range(0.01f, 0.03f);

        itemToBeConstructed.transform.SetPositionAndRotation(new Vector3(ghostPosition.x, ghostPosition.y, ghostPosition.z + randomOffset), ghostRotation);

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

        if (itemToBeConstructed.name == "FoundationModel")
        {
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            itemToBeConstructed.tag = "PlacedFoundation";

            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();
        }
        else if (itemToBeConstructed.name == "WallModel")
        {
            itemToBeConstructed.tag = "PlacedWall";
            DestroyItem(selectedGhost);
        }
        else
        {
            itemToBeConstructed.tag = "PlacedFloor";
            DestroyItem(selectedGhost);
        }

        itemToBeConstructed = null;
        inConstrucionMode = false;
    }

    private void PlaceItemFreeStyle()
    {
        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
        itemToBeConstructed.tag = "PlacedFoundation";
        itemToBeConstructed.GetComponent<Constructable>().enabled = false;
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        GetAllGhosts(itemToBeConstructed);
        PerformGhostDeletionScan();
        itemToBeConstructed = null;
        inConstrucionMode = false;
    }

    void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();
    }

    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }

        return false;
    }
}