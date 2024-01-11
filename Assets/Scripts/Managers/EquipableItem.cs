using System.Collections;
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
            StartCoroutine(SwingSoundDelay());
            GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                StartCoroutine(ChopSoundDelay());
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
            anim.SetTrigger("Hit");
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator ChopSoundDelay()
    {
        yield return new WaitForSeconds(0.35f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
    }
}
