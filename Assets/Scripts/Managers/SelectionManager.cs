using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {  get; set; }

    public GameObject selectedObject;

    [Header("UI")]
    public GameObject interaction_info_UI;
    public Image centerDotImage;
    public Image handIcon;
    Text interaction_Text;

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

            Animal animal = selectionTransform.GetComponent<Animal>();
            if (animal && animal.playerInRange)
            {
                interaction_Text.text = animal.animalName;
                interaction_info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }
            else
            {
                interaction_Text.text = "";
                interaction_info_UI.SetActive(false);
            }

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
            else
            {
                onTarget = false;
                handIsVisible = false;
            }
        }
        else
        {
            onTarget = false;
            interaction_info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
            handIsVisible = false;
        }
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