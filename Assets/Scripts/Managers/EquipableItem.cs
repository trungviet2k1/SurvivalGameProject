using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.handIsVisible == false)
        {
            anim.SetTrigger("Hit");
        }
    }
}
