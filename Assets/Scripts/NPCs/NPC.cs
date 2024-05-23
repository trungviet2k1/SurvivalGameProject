using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isInteractionWithPlayer;

    [Header("Name")]
    public string NPCName;

    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    private Animator anim;

    public string GetItemName()
    {
        return NPCName;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    public void StartConversation()
    {
        isInteractionWithPlayer = true;
        anim.SetBool("Interaction", true);
        DialogueSystem.Instance.OpenDialogueUI();
        DialogueSystem.Instance.dialogueText.text = "Hello there! How are you today?";
        DialogueSystem.Instance.optBtn_1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "I'm fine";
        DialogueSystem.Instance.optBtn_2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "So bad!";
        DialogueSystem.Instance.optBtn_1.onClick.AddListener(() =>{
            DialogueSystem.Instance.CloseDialogueUI();
            isInteractionWithPlayer = false;
            anim.SetBool("Interaction", false);
        });
    }

    public void EndConversation()
    {
        isInteractionWithPlayer = false;
        anim.SetBool("Interaction", false);
        DialogueSystem.Instance.CloseDialogueUI();
    }
}
