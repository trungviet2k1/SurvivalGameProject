using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpareBagSystem : MonoBehaviour
{
    public static SpareBagSystem Instance { get; set; }

    [Header("Spare Bag")]
    public GameObject spareBagScreenUI;
    public List<GameObject> slotList = new List<GameObject>();

    [Header("List Items")]
    public List<string> itemList = new List<string>();

    [Header("Notification")]
    public GameObject alert;

    [HideInInspector] public bool isSpareBagOpen;
    Button button;

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
        button = GetComponent<Button>();
        ShowSpareBag();
        PopulateSlotList();
    }

    private void ShowSpareBag()
    {
        if (InventorySystem.Instance.isOpen == true)
        {
            button.onClick.AddListener(ToggleSpareBagScreen);

            if (spareBagScreenUI != null)
            {
                spareBagScreenUI.SetActive(false);
                isSpareBagOpen = false;
            }
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in spareBagScreenUI.transform)
        {
            if (child.CompareTag("SpareBagSlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    public void ToggleSpareBagScreen()
    {
        if (spareBagScreenUI != null)
        {
            isSpareBagOpen = !isSpareBagOpen;
            spareBagScreenUI.SetActive(isSpareBagOpen);
        }
    }

    public void TransferToSpareBag(GameObject item)
    {
        if (CheckSpareBagSlotsAvailable())
        {
            alert.SetActive(false);
            GameObject spareBagSlot = FindNextEmptySpareBagSlot();

            item.transform.SetParent(spareBagSlot.transform);
            item.transform.position = spareBagSlot.transform.position;

            ReCalculateList();
            InventorySystem.Instance.ReCalculateList();
        }
        else
        {
            alert.SetActive(true);
            alert.GetComponent<CanvasGroup>().alpha = 0;
            alert.GetComponent<CanvasGroup>().DOFade(1, 1.5f).SetEase(Ease.OutQuad);

            DOVirtual.DelayedCall(2f, () => {
                alert.GetComponent<CanvasGroup>().DOFade(0, 1.5f).SetEase(Ease.InQuad);
                alert.SetActive(false);
            });
        }
    }

    private GameObject FindNextEmptySpareBagSlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckSpareBagSlotsAvailable()
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot += 1;
            }
        }

        return emptySlot > 0;
    }

    public void ReCalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }
}