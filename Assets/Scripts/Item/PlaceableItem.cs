using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOverlappingItems;
    public bool isValidToBeBuild;

    [SerializeField] BoxCollider solidCollider;
    private Outlines outlines;


    void Start()
    {
        outlines = GetComponent<Outlines>();
    }

    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuild = true;
        }
        else
        {
            isValidToBeBuild = false;
        }

        var boxHeight = transform.lossyScale.y;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, boxHeight * 0.5f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
            }
        }

        if (other.CompareTag("Plant") || other.CompareTag("PickAble")||
            other.CompareTag("Animal") || other.CompareTag("Stone"))
        {
            isOverlappingItems = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            isGrounded = false;
        }

        if (other.CompareTag("Plant") || other.CompareTag("PickAble") ||
            other.CompareTag("Animal") || other.CompareTag("Stone")
            && PlacementSystem.Instance.inPlacementMode)
        {
            isOverlappingItems = false;
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
