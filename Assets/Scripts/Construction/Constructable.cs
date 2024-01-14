using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    [Header("Materials")]
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial;
    private Renderer mRenderer;

    [Header("Box collider")]
    public BoxCollider solidCollider;

    [Header("List of ghost items")]
    public List<GameObject> ghostList;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isOverLappingItems;
    [HideInInspector] public bool isValidToBeBuilt;
    [HideInInspector] public bool detectedGhostMember;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
        mRenderer.material = defaultMaterial;

        foreach (Transform child in transform)
        {
            ghostList.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (isGrounded && isOverLappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && gameObject.CompareTag("ActiveConstructable"))
        {
            isGrounded = true;
        }

        if (other.CompareTag("Plant") || other.CompareTag("PickAble") && gameObject.CompareTag("ActiveConstructable"))
            {
            isOverLappingItems = true;
        }

        if (other.CompareTag("Ghost") && gameObject.CompareTag("ActiveConstructable"))
        {
            detectedGhostMember = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && gameObject.CompareTag("ActiveConstructable"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Plant") || other.CompareTag("PickAble") && gameObject.CompareTag("ActiveConstructable"))
        {
            isOverLappingItems = false;
        }

        if (other.CompareTag("Ghost") && gameObject.CompareTag("ActiveConstructable"))
        {
            detectedGhostMember = false;
        }
    }

    public void SetInvalidColor()
    {
        if (mRenderer != null)
        {
            mRenderer.material = redMaterial;
        }
    }

    public void SetValidColor()
    {
        if (mRenderer != null)
        {
            mRenderer.material = greenMaterial;
        }
    }

    public void SetDefaultColor()
    {
        if (mRenderer != null)
        {
            mRenderer.material = defaultMaterial;
        }
    }

    public void ExtractGhostMembers()
    {
        foreach (GameObject ghost in ghostList)
        {
            ghost.transform.SetParent(transform.parent, true);
            ghost.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
            ghost.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }
    }
}