using System.Collections;
using System.Collections.Generic;
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

    [Header("CampFire")]
    public GameObject selectedCampFire;

    [Header("Soil")]
    public GameObject selectedSoil;
    
    [Header("Fruit")]
    public GameObject selectedFruit;

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

            CampFire campFire = selectionTransform.GetComponent<CampFire>();

            if (campFire && campFire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_Text.text = "[E] Interact";
                interaction_info_UI.SetActive(true);

                selectedCampFire = campFire.gameObject;

                if (Input.GetKey(KeyCode.E) && campFire.isCooking == false)
                {
                    campFire.OpenUI();
                }
            }
            else
            {
                if (selectedCampFire != null)
                {
                    selectedCampFire = null;
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

            Soil soil = selectionTransform.GetComponent<Soil>();

            if (soil && soil.playerInRange)
            {

                if (soil.isEmpty && EquipSystem.Instance.IsPlayerHoldingSeed())
                {
                    string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] { "Seed" }, System.StringSplitOptions.None)[0];
                    interaction_Text.text = "[E] To plant " + onlyPlantName;
                    interaction_info_UI.SetActive(true);

                    if (Input.GetKey(KeyCode.E))
                    {
                        soil.PlantSeed();
                        Destroy(EquipSystem.Instance.selectedItem);
                        Destroy(EquipSystem.Instance.selectedItemModel);
                    }
                }
                else if (soil.isEmpty)
                {
                    interaction_Text.text = "Soil";
                    interaction_info_UI.SetActive(true);
                }
                else
                {
                    if (EquipSystem.Instance.IsPlayerHoldingWateringCan())
                    {
                        if (soil.currentPlant.isWatered)
                        {
                            interaction_Text.text = soil.plantName + " Plant";
                            interaction_info_UI.SetActive(true);
                        }
                        else
                        {
                            interaction_Text.text = "Use Watering Can";
                            interaction_info_UI.SetActive(true);

                            if (Input.GetMouseButtonDown(0))
                            {
                                SoundManager.Instance.wateringChanel.PlayOneShot(SoundManager.Instance.wateringCanSound);
                                soil.currentPlant.isWatered = true;
                                soil.MakeSoilWatered();
                            }

                        }
                    }
                    else
                    {
                        interaction_Text.text = soil.plantName + " Plant";
                        interaction_info_UI.SetActive(true);
                    }
                }

                selectedSoil = soil.gameObject;
            }
            else
            {
                if (selectedSoil != null)
                {
                    selectedSoil = null;
                }
            }

            Fruits fruits = selectionTransform.GetComponent<Fruits>();

            if (fruits && fruits.playerInRange)
            {
                interaction_Text.text = fruits.fruitName + " Plant";
                interaction_info_UI.SetActive(true);

                selectedFruit = fruits.gameObject;
            }
            else
            {
                if (selectedCampFire != null)
                {
                    selectedCampFire = null;
                }
            }

            if (!interacable && !animal)
            {
                onTarget = false;
                handIsVisible = false;
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!interacable && !animal && !choppeableTree && !storageBox && !campFire && !soil && !fruits)
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