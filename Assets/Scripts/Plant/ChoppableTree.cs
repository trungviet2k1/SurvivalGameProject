using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    [Header("Tree Status")]
    public float treeMaxHealth;
    public float treeHealth;

    [Header("Consuming")]
    public float caloriesSpentChoppingWood = 20;
    
    [Header("Animation")]
    public Animator anim;

    [HideInInspector] public bool playerInRange;
    [HideInInspector] public bool canBeChopped;
    private bool isBeingChopped = false;

    void Start()
    {
        treeHealth = treeMaxHealth;
        anim = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetHit()
    {
        if (!isBeingChopped && canBeChopped)
        {
            StartCoroutine(Hit());
        }
    }

    public IEnumerator Hit()
    {
        isBeingChopped = true;
        yield return new WaitForSeconds(0.6f);
        anim.SetTrigger("Shake");
        treeHealth -= 2;

        PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

        if (treeHealth <= 0)
        {
            TreeIsDead();
        }

        isBeingChopped = false;
    }

    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        if (SelectionManager.Instance.selectedObject != null)
        {
            string itemName = SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().GetItemName();
            Dictionary<string, string> treePrefabMap = new Dictionary<string, string>()
        {
            { "Birch tree", "ChoppedBirchTree" },
            { "Oak tree", "ChoppedOakTree" },
            { "Deciduous tree", "ChoppedDeciduousTree" }
        };

            if (treePrefabMap.ContainsKey(itemName))
            {
                string prefabName = treePrefabMap[itemName];
                GameObject choppedTreePrefab = Instantiate(Resources.Load<GameObject>(prefabName),
                    new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));
            }
        }
    }

    void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
}