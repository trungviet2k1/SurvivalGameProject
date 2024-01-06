using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if (!gameObject.CompareTag("Animal") && !gameObject.CompareTag("Plane") && SelectionManager.Instance.onTarget)
        {
            if (Input.GetKeyDown(KeyCode.F) && playerInRange)
            {
                Debug.Log("Item added to inventory");
                Destroy(gameObject);
            }
        }
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
}
