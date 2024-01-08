using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitive = 100f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (InventorySystem.instance.isOpen == false && CraftingSystem.instance.isOpen == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitive * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitive * Time.deltaTime;

            //Control rotation around x axis (Look up and down)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Control rotation around y axis (Look up and down)
            yRotation += mouseX;

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
