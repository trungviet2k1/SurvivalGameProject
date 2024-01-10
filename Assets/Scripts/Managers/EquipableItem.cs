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
            GameObject selectedTree = SelectionManager.Instance.selectedTree;
            
            if (selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }

            anim.SetTrigger("Hit");
        }
    }
}
