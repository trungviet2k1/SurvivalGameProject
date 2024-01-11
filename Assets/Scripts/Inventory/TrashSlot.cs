using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject trashAlertUI;
    public Sprite trash_closed;
    public Sprite trash_opened;

    private Text textModify;
    private Image imageComponent;
    Button YesBtn, NoBtn;

    GameObject DraggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    GameObject itemToBeDeleted;

    public string ItemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    void Start()
    {
        imageComponent = transform.Find("Background").GetComponent<Image>();
        textModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();

        YesBtn = trashAlertUI.transform.Find("Yes").GetComponent<Button>();
        YesBtn.onClick.AddListener(delegate { DeleteItem(); });

        NoBtn = trashAlertUI.transform.Find("No").GetComponent<Button>();
        NoBtn.onClick.AddListener(delegate { CancelDeletetion(); });
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = DraggedItem.gameObject;
            StartCoroutine(NotifyBeforeDeletion());
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (DraggedItem != null && DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_opened;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (DraggedItem != null && DraggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_closed;
        }
    }

    IEnumerator NotifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textModify.text = "Throw away this " + ItemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void DeleteItem()
    {
        imageComponent.sprite = trash_closed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();
        trashAlertUI.SetActive(false);
    }

    private void CancelDeletetion()
    {
        imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }
}