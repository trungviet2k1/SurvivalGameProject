using UnityEngine;
using UnityEngine.UI;

public class ShowSpareBag : MonoBehaviour
{
    public GameObject spareBagScreenUI;
    [HideInInspector] public bool isSpareBagOpen;

    void Start()
    {
        Button button = GetComponent<Button>();

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

    public void ToggleSpareBagScreen()
    {
        if (spareBagScreenUI != null)
        {
            isSpareBagOpen = !isSpareBagOpen;
            spareBagScreenUI.SetActive(isSpareBagOpen);
        }
    }
}