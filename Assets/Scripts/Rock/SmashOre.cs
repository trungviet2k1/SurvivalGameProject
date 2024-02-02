using System.Collections;
using UnityEngine;

public enum OreType
{
    Stone,
    RawIron
}

[RequireComponent(typeof(MeshCollider))]
public class SmashOre : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("Rock Status")]
    public float rockMaxHealth;
    public float rockHealth;

    [Header("Ore Type")]
    public OreType oreType;

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
        int minStones;
        int maxStones;

        if (oreType == OreType.Stone)
        {
            minStones = 10;
            maxStones = 20;
        }
        else
        {
            minStones = 5;
            maxStones = 10;
        }

        int numberOfStones = Random.Range(minStones, maxStones + 1);

        GameObject items = GetComponentInParent<EnvironmentManager>().allRocks;

        for (int i = 0; i < numberOfStones; i++)
        {
            float yOffset = i * 0.2f;
            string prefabName = (oreType == OreType.Stone) ? "Stone_Model" : "Raw Iron_Model";
            GameObject oreModel = Instantiate(Resources.Load(prefabName),
                transform.position + Vector3.up * yOffset, Quaternion.identity) as GameObject;
            oreModel.transform.parent = items.transform;
        }

        Destroy(transform.gameObject);
        canBeSmashed = false;
        SelectionManager.Instance.selectedRock = null;
        SelectionManager.Instance.chopHolder.SetActive(false);
    }
}