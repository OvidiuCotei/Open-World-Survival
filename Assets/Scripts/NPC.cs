using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Multilanguage;
using UnityEngine.UI;


namespace OvyKode
{
    public class NPC : MonoBehaviour
    {
        public bool playerInRange;
        public string itemNameKey;
        public string itemName;
        public bool isTaklkingWithPlayer;

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

        public string GetItemName()
        {
            return LocalizationManager.Instance.GetTextForKey(itemNameKey);
        }

        public void StartConversation()
        {
            isTaklkingWithPlayer = true;
            DialogSystem.Instance.OpenDialogUI();
            DialogSystem.Instance.dialogText.text = "Hello there";
            DialogSystem.Instance.option1Btn.transform.Find("Text").GetComponent<Text>().text = "Bye";
            DialogSystem.Instance.option1Btn.onClick.AddListener(() =>
            {
                DialogSystem.Instance.CloseDialogUI();
                isTaklkingWithPlayer = false;
            });
        }
    }
}