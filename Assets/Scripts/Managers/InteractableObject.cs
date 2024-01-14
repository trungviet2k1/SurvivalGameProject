using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    [Header("Name")]
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if (!gameObject.CompareTag("Animal") && !gameObject.CompareTag("Plant") 
            && !gameObject.CompareTag("Stone"))
        {
            if (Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.selectedObject == gameObject)
            {
                if (InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    InventorySystem.Instance.AddToInventory(ItemName);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
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