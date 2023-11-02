using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    [RequireComponent(typeof(Animator))]
    public class EquippableItem : MonoBehaviour
    {
        private Animator animator;
        public float damage = 1;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false && SelectionManager.Instance.handIsVisible == false && !ConstructionManager.Instance.inConstructionMode)
            {
                StartCoroutine(SwingSoundDelay());
                animator.SetTrigger("Hit");
            }
        }

        public void GetHit()
        {
            GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                SoundManager.Instance.PlaySoundSource(SoundManager.Instance.chopSound, transform.position);
                selectedTree.GetComponent<ChoppableTree>().GetHit(damage);
            }
        }

        private IEnumerator SwingSoundDelay()
        {
            yield return new WaitForSeconds(0.2f);
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
    }
}