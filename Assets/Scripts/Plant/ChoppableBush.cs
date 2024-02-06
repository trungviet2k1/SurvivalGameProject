using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableBush : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("Tree Status")]
    public float treeMaxHealth;
    public float treeHealth;

    [HideInInspector] public bool playerInRange;
    [HideInInspector] public bool canBeChopped;
    private bool isBeingChopped = false;

    void Start()
    {
        treeHealth = treeMaxHealth;
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

        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
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
        treeHealth -= 2;

        if (treeHealth <= 0)
        {
            TreeIsDead();
        }

        isBeingChopped = false;
    }

    void TreeIsDead()
    {
        int numberOfSticks = Random.Range(2, 4);

        GameObject items = GetComponentInParent<EnvironmentManager>().allItems;

        for (int i = 0; i < numberOfSticks; i++)
        {
            GameObject stickModel = Instantiate(Resources.Load("Stick_Model"), transform.position + Vector3.up, Quaternion.identity) as GameObject;
            stickModel.transform.parent = items.transform;
            stickModel.name = "Stick_Model(Clone)" + (i == 0 ? "" : "(" + i + ")");
        }

        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.SetActive(false);
    }
}