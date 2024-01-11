using UnityEngine;

public class CraftingScreenController : MonoBehaviour
{
    public CanvasGroup toolsCanvasGroup;
    public GameObject toolsContent;
    public CanvasGroup architectureCanvasGroup;
    public GameObject architectureContent;

    void Start()
    {
        toolsCanvasGroup.alpha = 1f;
        architectureCanvasGroup.alpha = 0.7f;
        architectureContent.SetActive(false);
    }

    public void OnToolsButtonClick()
    {
        toolsCanvasGroup.alpha = 1f;
        toolsContent.SetActive(true);

        architectureCanvasGroup.alpha = 0.7f;
        architectureContent.SetActive(false);
    }

    public void OnArchitectureButtonClick()
    {
        architectureCanvasGroup.alpha = 1f;
        architectureContent.SetActive(true);

        toolsCanvasGroup.alpha = 0.7f;
        toolsContent.SetActive(false);
    }
}