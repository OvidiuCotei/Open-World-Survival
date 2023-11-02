using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    [RequireComponent(typeof(BoxCollider))]
    public class ChoppableTree : MonoBehaviour
    {
        public bool playerInRange;
        public bool canBeChopped;
        public float treeMaxHealth = 10;
        public float treeHealth;
        private Vector3 initialPosition;
        public GameObject tree;
        public float animationDuration = 0.5f;
        public float caloriesSpentChoppingWood = 20;

        private void Start()
        {
            treeHealth = treeMaxHealth;
        }

        private void Update()
        {
            if(canBeChopped)
            {
                GlobalState.Instance.resourceHealth = treeHealth;
                GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() is var player && player != null)
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() is var player && player != null)
            {
                playerInRange = false;
            }
        }

        public void GetHit(float damage)
        {
            StartCoroutine(ShakeAnimation());
            treeHealth -= damage;
            PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

            if (treeHealth <= 0)
            {
                treeHealth = 0;
                TreeIsDead();
            }
        }

        private IEnumerator ShakeAnimation()
        {
            float elapsedTime = 0f;
            initialPosition = tree.transform.position;

            while (elapsedTime < animationDuration)
            {
                float normalizedTime = elapsedTime / animationDuration;
                float shakeIntensity = Mathf.Lerp(0f, 0.01f, Mathf.Sin(normalizedTime * Mathf.PI));

                float x = initialPosition.x + Random.Range(-shakeIntensity, shakeIntensity);
                float z = initialPosition.z + Random.Range(-shakeIntensity, shakeIntensity);

                tree.transform.position = new Vector3(x, initialPosition.y, z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            tree.transform.position = initialPosition; // Reset the tree's position to the initial position after the animation
        }

        private void TreeIsDead()
        {
            Vector3 treePosition = transform.position;
            Destroy(transform.parent.transform.parent.gameObject);
            canBeChopped = false;
            SelectionManager.Instance.selectedTree = null;
            SelectionManager.Instance.chopHolder.gameObject.SetActive(false);
            GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), new Vector3(treePosition.x, treePosition.y + 0.01f, treePosition.z), Quaternion.Euler(0f, 0f, 0f));    
        }
    }
}