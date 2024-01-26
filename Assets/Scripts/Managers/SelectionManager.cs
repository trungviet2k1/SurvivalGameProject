using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject selectedObject;

    [Header("UI")]
    public GameObject interaction_info_UI;
    public Image centerDotImage;
    public Image handIcon;
    Text interaction_Text;

    [Header("Storage Box")]
    public GameObject selectedStorageBox;

    [Header("Tree")]
    public GameObject selectedTree;
    public GameObject chopHolder;

    [HideInInspector] public bool onTarget;
    [HideInInspector] public bool handIsVisible;

    void Start()
    {
        onTarget = false;
        interaction_Text = interaction_info_UI.GetComponent<Text>();
    }

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

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selectionTransform = hit.transform;

            ChoppableTree choppeableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppeableTree && choppeableTree.playerInRange)
            {
                choppeableTree.canBeChopped = true;
                selectedTree = choppeableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            InteractableObject interacable = selectionTransform.GetComponent<InteractableObject>();

            if (interacable && interacable.playerInRange)
            {
                onTarget = true;
                selectedObject = interacable.gameObject;
                interaction_Text.text = interacable.GetItemName();
                interaction_info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;


                if (interacable.CompareTag("PickAble"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisible = true;
                }
                else
                {
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;
                }
            }

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();

            if (storageBox && storageBox.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_Text.text = "[E] Open";
                interaction_info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if (Input.GetKey(KeyCode.E))
                {
                    StorageSystem.Instance.OpenBox(storageBox);
                }
            }
            else
            {
                if (selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }

            Animal animal = selectionTransform.GetComponent<Animal>();
            if (animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interaction_Text.text = "[F] Loot";
                    interaction_info_UI.SetActive(true);
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisible = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        LootAble lootable = animal.GetComponent<LootAble>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_Text.text = animal.animalName;
                    interaction_info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                    }
                }
            }

            if (!interacable && !animal)
            {
                onTarget = false;
                handIsVisible = false;
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!interacable && !animal && !choppeableTree && !storageBox)
            {
                interaction_Text.text = "";
                interaction_info_UI.SetActive(false);
            }
        }
    }

    private void Loot(LootAble lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootReceived> receivedLoot = new();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount != 0)
                {
                    LootReceived lt = new()
                    {
                        item = loot.item,
                        amount = lootAmount
                    };
                    receivedLoot.Add(lt);
                }
            }

            lootable.finalLoot = receivedLoot;
            lootable.wasLootCalculated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootReceived loot in lootable.finalLoot)
        {
            for (int i = 0; i < loot.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(loot.item.name + "_Model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                    Quaternion.Euler(0, 0, 0));
            }
        }

        if (lootable.GetComponent<Animal>()) { }
        {
            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
        }

        Destroy(lootable.gameObject);
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_info_UI.SetActive(false);
        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_info_UI.SetActive(true);
    }
}