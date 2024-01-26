using UnityEngine;

public class CraftingScreenController : MonoBehaviour
{
    [Header("Tools")]
    public CanvasGroup toolsButtonCanvasGroup;
    public GameObject toolsMenu;

    [Header("Resources")]
    public CanvasGroup resourcesButtonCanvasGroup;
    public GameObject resourcesMenu;

    [Header("Constructions")]
    public CanvasGroup constructionsButtonCanvasGroup;
    public GameObject constructionsMenu;

    void Start()
    {
        SetButtonAlpha(toolsButtonCanvasGroup, 0.5f);
        SetButtonAlpha(resourcesButtonCanvasGroup, 0.5f);
        SetButtonAlpha(constructionsButtonCanvasGroup, 0.5f);
    }

    public void OnToolsButtonClick()
    {
        SetButtonAlpha(toolsButtonCanvasGroup, 1f);
        SetButtonAlpha(resourcesButtonCanvasGroup, 0.5f);
        SetButtonAlpha(constructionsButtonCanvasGroup, 0.5f);
        CraftingMenu.Instance.OnCraftingButtonClick(toolsButtonCanvasGroup, toolsMenu, null);
    }

    public void OnResourcesButtonClick()
    {
        SetButtonAlpha(toolsButtonCanvasGroup, 0.5f);
        SetButtonAlpha(resourcesButtonCanvasGroup, 1f);
        SetButtonAlpha(constructionsButtonCanvasGroup, 0.5f);
        CraftingMenu.Instance.OnCraftingButtonClick(resourcesButtonCanvasGroup, resourcesMenu, null);
    }

    public void OnConstructionsButtonClick()
    {
        SetButtonAlpha(toolsButtonCanvasGroup, 0.5f);
        SetButtonAlpha(resourcesButtonCanvasGroup, 0.5f);
        SetButtonAlpha(constructionsButtonCanvasGroup, 1f);
        CraftingMenu.Instance.OnCraftingButtonClick(constructionsButtonCanvasGroup, constructionsMenu, null);
    }

    private void SetButtonAlpha(CanvasGroup buttonCanvasGroup, float alpha)
    {
        buttonCanvasGroup.alpha = alpha;
    }
}