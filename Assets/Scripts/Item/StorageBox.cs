using System.Collections.Generic;
using UnityEngine;

public class StorageBox : MonoBehaviour
{
    [Header("List of Storage Box")]
    public List<string> items;

    [HideInInspector] public bool playerInRange;

    public enum BoxType
    {
        smallBox,
        bigBox
    }

    public BoxType thisBoxType;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
