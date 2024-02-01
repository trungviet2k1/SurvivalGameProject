using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator anim;
    [HideInInspector] public bool swingWait = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && MenuManager.Instance.isMenuOpen == false
            && SelectionManager.Instance.handIsVisible == false
            && swingWait == false
            && !ConstructionManager.Instance.inConstrucionMode
            )
        {
            swingWait = true;
            StartCoroutine(SwingSoundDelay());
            anim.SetTrigger("Hit");
            StartCoroutine(NewSwingDelay());
        }
    }

    void GetHitTree()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    void GetHitBush()
    {
        GameObject selectedBush = SelectionManager.Instance.selectedBush;

        if (selectedBush != null)
        {
            selectedBush.GetComponent<ChoppableBush>().GetHit();
        }
    }

    void GetHitRock()
    {
        GameObject selectedRock = SelectionManager.Instance.selectedRock;

        if (selectedRock != null)
        {
            selectedRock.GetComponent<SmashRock>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.3f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }
}