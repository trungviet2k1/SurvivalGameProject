using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator anim;
    private bool swingWait = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.handIsVisible == false
            && swingWait == false
            && !ConstructionManager.Instance.inConstrucionMode)
        {
            swingWait = true;
            StartCoroutine(SwingSoundDelay());
            anim.SetTrigger("Hit");
            StartCoroutine(NewSwingDelay());
            swingWait = false;
        }
    }

    void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(0.3f);
        GetHit();
    }
}