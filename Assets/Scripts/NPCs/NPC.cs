using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isInteractionWithPlayer;

    [Header("Name")]
    public string NPCName;

    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("Dialogue")]
    TextMeshProUGUI npcDialogueText;
    Button optButton_1;
    TextMeshProUGUI optBtnText_1;
    Button optButton_2;
    TextMeshProUGUI optBtnText_2;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialogue;

    private Animator animator;

    public string GetItemName()
    {
        return NPCName;
    }

    void Start()
    {
        npcDialogueText = DialogueSystem.Instance.dialogueText;
        optButton_1 = DialogueSystem.Instance.optBtn_1;
        optButton_2 = DialogueSystem.Instance.optBtn_2;
        optBtnText_1 = DialogueSystem.Instance.optBtn_1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        optBtnText_2 = DialogueSystem.Instance.optBtn_2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        animator = GetComponent<Animator>();
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
        LookAtPlayer();
        animator.SetBool("Interaction", true);

        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialogue();
            currentDialogue = 0;
        }
        else
        {
            if (currentActiveQuest.declined)
            {
                DialogueSystem.Instance.OpenDialogueUI();
                npcDialogueText.text = currentActiveQuest.info.comebackAfterDecline;
                SetAcceptAndDeclineOptions();
            }

            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirementCompleted())
                {
                    SubmitRequiredItem();
                    DialogueSystem.Instance.OpenDialogueUI();
                    npcDialogueText.text = currentActiveQuest.info.comebackCompleted;
                    ReceiveRewardAndCompleteQuest();
                }
                else
                {
                    DialogueSystem.Instance.OpenDialogueUI();
                    npcDialogueText.text = currentActiveQuest.info.comebackInProgress;
                    CloseDialogueUI();
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                DialogueSystem.Instance.OpenDialogueUI();
                npcDialogueText.text = currentActiveQuest.info.finalWords;
                CloseDialogueUI();
            }

            if (currentActiveQuest.initialDialogueCompleted == false)
            {
                StartQuestInitialDialogue();
            }
        }
    }

    private void SubmitRequiredItem()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (secondRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondRequiredItem, secondRequiredAmount);
        }
    }

    private bool AreQuestRequirementCompleted()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCounter++;
            }
        }

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartQuestInitialDialogue()
    {
        DialogueSystem.Instance.OpenDialogueUI();

        npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];
        optBtnText_1.text = "Next";
        optButton_1.onClick.RemoveAllListeners();
        optButton_1.onClick.AddListener(() =>
        {
            currentDialogue++;
            CheckIfDialogueDone();
        });

        optButton_2.gameObject.SetActive(false);
    }

    private void CheckIfDialogueDone()
    {
        if (currentDialogue == currentActiveQuest.info.initialDialogue.Count - 1)
        {
            npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];
            currentActiveQuest.initialDialogueCompleted = true;
            SetAcceptAndDeclineOptions();
        }
        else
        {
            npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];

            optBtnText_1.text = "Next";
            optButton_1.onClick.RemoveAllListeners();
            optButton_1.onClick.AddListener(() => {
                currentDialogue++;
                CheckIfDialogueDone();
            });
        }
    }

    private void AcceptedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogueText.text = currentActiveQuest.info.comebackCompleted;
            ReceiveRewardAndCompleteQuest();
            optButton_2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogueText.text = currentActiveQuest.info.acceptAnswer;
            CloseDialogueUI();
            optButton_2.gameObject.SetActive(false);
        }
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        QuestManager.Instance.MakeQuestCompleted(currentActiveQuest);
        currentActiveQuest.isCompleted = true;
        var coinsReceived = currentActiveQuest.info.coinReward;
        CurrencyManager.Instance.AddGold(coinsReceived);

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        }

        activeQuestIndex++;

        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialogue = 0;
            DialogueSystem.Instance.CloseDialogueUI();
            animator.SetBool("Interaction", false);
            isInteractionWithPlayer = false;
        }
        else
        {
            DialogueSystem.Instance.CloseDialogueUI();
            animator.SetBool("Interaction", false);
            isInteractionWithPlayer = false;
            Debug.Log("No more questions!");
        }
    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;
        npcDialogueText.text = currentActiveQuest.info.declineAnswer;
        CloseDialogueUI();
        optButton_2.gameObject.SetActive(false);
    }

    private void SetAcceptAndDeclineOptions()
    {
        optBtnText_1.text = currentActiveQuest.info.acceptOption;
        optButton_1.onClick.RemoveAllListeners();
        optButton_1.onClick.AddListener(() =>
        {
            AcceptedQuest();
        });

        optButton_2.gameObject.SetActive(true);
        optBtnText_2.text = currentActiveQuest.info.declineOption;
        optButton_2.onClick.RemoveAllListeners();
        optButton_2.onClick.AddListener(() =>
        {
            DeclinedQuest();
        });
    }

    private void TakeReward()
    {
        optBtnText_1.text = "[Take reward]";
        optButton_1.onClick.RemoveAllListeners();
        optButton_1.onClick.AddListener(() =>
        {
            ReceiveRewardAndCompleteQuest();
        });
    }

    private void CloseDialogueUI()
    {
        optBtnText_1.text = "[Close]";
        optButton_1.onClick.RemoveAllListeners();
        optButton_1.onClick.AddListener(() =>
        {
            DialogueSystem.Instance.CloseDialogueUI();
            animator.SetBool("Interaction", false);
            isInteractionWithPlayer = false;
        });
    }

    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}