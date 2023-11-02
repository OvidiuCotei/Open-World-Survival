using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class Constructable : MonoBehaviour
    {
        public bool isGrounded;
        public bool isOverlappingItems;
        public bool isValidToBeBuilt;
        public bool detectedGhostMemeber;
        private Renderer mRenderer;
        public Material redMaterial;
        public Material greenMaterial;
        public Material defaultMaterial;
        public List<GameObject> ghostList = new List<GameObject>();
        public BoxCollider solidCollider; // We need to drag this collider manualy into the inspector

        private void Start()
        {
            mRenderer = GetComponent<Renderer>();
            mRenderer.material = defaultMaterial;

            foreach (Transform child in transform)
            {
                ghostList.Add(child.gameObject);
            }
        }

        private void Update()
        {
            if (isGrounded && isOverlappingItems == false)
            {
                isValidToBeBuilt = true;
            }
            else
            {
                isValidToBeBuilt = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground") && gameObject.CompareTag("ActiveConstructable"))
            {
                isGrounded = true;
            }

            if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("ActiveConstructable"))
            {

                isOverlappingItems = true;
            }

            if (other.gameObject.CompareTag("Ghost") && gameObject.CompareTag("ActiveConstructable"))
            {
                detectedGhostMemeber = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground") && gameObject.CompareTag("ActiveConstructable"))
            {
                isGrounded = false;
            }

            if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("ActiveConstructable"))
            {

                isOverlappingItems = false;
            }

            if (other.gameObject.CompareTag("Ghost") && gameObject.CompareTag("ActiveConstructable"))
            {
                detectedGhostMemeber = false;
            }
        }

        public void SetInvalidColor()
        {
            if (mRenderer != null)
            {
                mRenderer.material = redMaterial;
            }
        }

        public void SetValidColor()
        {
            mRenderer.material = greenMaterial;
        }

        public void SetDefaultColor()
        {
            mRenderer.material = defaultMaterial;
        }

        public void ExtractGhostMembers()
        {
            foreach (GameObject item in ghostList)
            {
                item.transform.SetParent(transform.parent, true);
                item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
                item.gameObject.GetComponent<GhostItem>().isPlaced = true;
            }
        }
    }
}