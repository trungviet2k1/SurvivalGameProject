﻿using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;

    [Header("Name")]
    public string ItemName;

    [Header("Range")]
    [SerializeField] float detectionRange = 10f;

    public string GetItemName()
    {
        return ItemName;
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

        if (!gameObject.CompareTag("Animal") && !gameObject.CompareTag("Plant") 
            && !gameObject.CompareTag("Stone"))
        {
            if (Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.selectedObject == gameObject)
            {
                if (InventorySystem.Instance.CheckSlotsAvailable(1))
                {
                    InventorySystem.Instance.AddToInventory(ItemName);
                    InventorySystem.Instance.itemPickedup.Add(gameObject.name);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
        }
    }
}