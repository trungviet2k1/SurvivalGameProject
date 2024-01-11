using UnityEngine;

public class CraftingScreenController : MonoBehaviour
{
    [Header("List of tools")]
    public CanvasGroup toolsCanvasGroup;
    public GameObject toolsMenu;

    [Header("List of architectures")]
    public CanvasGroup architectureCanvasGroup;
    public GameObject architectureMenu;

    void Start()
    {
        toolsCanvasGroup.alpha = 1f;
        architectureCanvasGroup.alpha = 0.7f;
        architectureMenu.SetActive(false);
    }

    public void OnToolsButtonClick()
    {
        toolsCanvasGroup.alpha = 1f;
        toolsMenu.SetActive(true);

        architectureCanvasGroup.alpha = 0.7f;
        architectureMenu.SetActive(false);
    }

    public void OnArchitectureButtonClick()
    {
        architectureCanvasGroup.alpha = 1f;
        architectureMenu.SetActive(true);

        toolsCanvasGroup.alpha = 0.7f;
        toolsMenu.SetActive(false);
    }
}