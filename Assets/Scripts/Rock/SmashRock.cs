using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class SmashRock : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("Rock Status")]
    public float rockMaxHealth;
    public float rockHealth;

    [HideInInspector] public bool playerInRange;
    [HideInInspector] public bool canBeSmashed;
    private bool isBeingSmashed = false;

    void Start()
    {
        rockHealth = rockMaxHealth;
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

        if (canBeSmashed)
        {
            GlobalState.Instance.resourceHealth = rockHealth;
            GlobalState.Instance.resourceMaxHealth = rockMaxHealth;
        }
    }

    public void GetHit()
    {
        if (!isBeingSmashed && canBeSmashed)
        {
            StartCoroutine(Hit());
        }
    }

    public IEnumerator Hit()
    {
        isBeingSmashed = true;
        yield return new WaitForSeconds(0.6f);
        rockHealth -= 4;

        if (rockHealth <= 0)
        {
            RockShattered();
        }

        isBeingSmashed = false;
    }

    void RockShattered()
    {
        int numberOfStones = Random.Range(10, 20);

        GameObject items = GetComponentInParent<EnvironmentManager>().allRocks;

        for (int i = 0; i < numberOfStones; i++)
        {
            float yOffset = i * 0.2f;
            GameObject stoneModel = Instantiate(Resources.Load("Stone_Model"),
                transform.position + Vector3.up * yOffset, Quaternion.identity) as GameObject;
            stoneModel.transform.parent = items.transform;
        }

        Destroy(transform.gameObject);
        canBeSmashed = false;
        SelectionManager.Instance.selectedRock = null;
        SelectionManager.Instance.chopHolder.SetActive(false);
    }
}