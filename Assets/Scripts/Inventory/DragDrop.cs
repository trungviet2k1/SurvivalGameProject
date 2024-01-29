using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;

        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
        itemBeingDragged = null;
        if (transform.parent == startParent || transform.parent == transform.root)
        {
            tempItemReference.SetActive(false);

            AlertDialogManager diaLogManager = FindObjectOfType<AlertDialogManager>();
            diaLogManager.ShowDialog("Drop this items?", (response) =>
            {
                if (response)
                {
                    DropItemIntoWorld(tempItemReference);
                }
                else
                {
                    transform.position = startPosition;
                    transform.SetParent(startParent);
                }

            });
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        InventorySystem.Instance.ReCalculateList();
        SpareBagSystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();
    }

    private void DropItemIntoWorld(GameObject tempItemReference)
    {
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];

        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        var itemObject = FindObjectOfType<EnvironmentManager>().gameObject.transform.Find("[Items]");
        item.transform.SetParent(itemObject.transform);

        DestroyImmediate(tempItemReference);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();
        SpareBagSystem.Instance.ReCalculateList();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (!SpareBagSystem.Instance.IsItemInSpareBag(gameObject))
            {
                SpareBagSystem.Instance.TransferToSpareBag(gameObject);
                CraftingSystem.Instance.RefreshNeedItems();
            }
        }
    }
}