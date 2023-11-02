using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Multilanguage;

namespace OvyKode
{
    public class InteractableObject : MonoBehaviour
    {

        public TAG tags;
        public bool playerInRange;
        public string itemNameKey;
        public string itemName;
        [HideInInspector] public string itemNameLanguage;

        private void Start()
        {
            itemNameLanguage = GetItemName();
        }

        private void Update()
        {
            if(Input.GetKeyDown(InputManager.Instance.MouseLeft) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
            {
                if(InventorySystem.Instance.ChechSlotsAvailable(1))
                {
                    InventorySystem.Instance.AddToInventory(itemName, itemNameLanguage);
                    InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("<color=orange>Inventory is full!</color>");
                }
            }
        }

        public string GetItemName()
        {
            return LocalizationManager.Instance.GetTextForKey(itemNameKey);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerMovement>() is var player && player != null)
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
    }
}