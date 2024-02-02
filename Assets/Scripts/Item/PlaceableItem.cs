using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    [Header("Building")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOverlappingItems;
    public bool isValidToBeBuild;

    [Header("Collider")]
    [SerializeField] BoxCollider solidCollider;
    private Outlines outlines;
    [HideInInspector] public bool playerInRange;

    void Start()
    {
        outlines = GetComponent<Outlines>();
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

        var boxHeight = solidCollider.size.y * transform.localScale.y;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, boxHeight * 0.5f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            isValidToBeBuild = !isOverlappingItems;
        }
        else
        {
            isGrounded = false;
            isValidToBeBuild = false;
        }
    }

    #region || ----- On Triggers ----- ||
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;

                isGrounded = true;
                isValidToBeBuild = !isOverlappingItems;
            }
        }

        if (other.CompareTag("Plant") || other.CompareTag("PickAble") ||
            other.CompareTag("Animal") || other.CompareTag("Stone"))
        {
            isOverlappingItems = true;
            isValidToBeBuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            isGrounded = false;
            isValidToBeBuild = false;
        }

        if ((other.CompareTag("Plant") || other.CompareTag("PickAble") ||
            other.CompareTag("Animal") || other.CompareTag("Stone")) && PlacementSystem.Instance.inPlacementMode)
        {
            isOverlappingItems = false;
            isValidToBeBuild = !isGrounded;
        }
    }

    #endregion

    #region || ----- Set Outline Colors ----- ||
    public void SetValidColor()
    {
        if (outlines != null)
        {
            outlines.enabled = true;
            outlines.OutlineColor = Color.green;
        }
    }

    public void SetInvalidColor()
    {
        if (outlines != null)
        {
            outlines.enabled = true;
            outlines.OutlineColor = Color.red;
        }
    }

    public void SetDefaultColor()
    {
        if (outlines != null)
        {
            outlines.enabled = false;
        }
    }

    #endregion
}