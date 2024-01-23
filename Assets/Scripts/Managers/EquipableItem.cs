using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator anim;
    private bool swingWait = false;
    private bool soundPlayed = false;

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
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            StartCoroutine(SwingSoundDelay());
            anim.SetTrigger("Hit");
            StartCoroutine(NewSwingDelay());
            soundPlayed = false;
        }
    }

    void OnHitAnimationComplete()
    {
        if (SelectionManager.Instance.selectedTree != null && !soundPlayed)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            soundPlayed = true;
        }
    }

    void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.3f);
        soundPlayed = false;
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(3f);
        GetHit();
    }
}