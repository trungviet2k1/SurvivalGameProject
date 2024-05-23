using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; set; }

    public TextMeshProUGUI dialogueText;

    public Button optBtn_1;
    public Button optBtn_2;

    public Canvas dialogueUI;
    public bool dialogueUIActive;

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

    public void OpenDialogueUI()
    {
        dialogueUI.gameObject.SetActive(true);
        dialogueUIActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogueUI()
    {
        dialogueUI.gameObject.SetActive(false);
        dialogueUIActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
