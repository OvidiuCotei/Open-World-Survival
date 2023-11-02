using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OvyKode
{
    public class TrashSlot : MonoBehaviour, IDropHandler
    {
        public GameObject trashAlertUI;
        private Text textToModify;
        private Button YesBTN, NoBTN;

        GameObject draggedItem
        {
            get
            {
                return DragDrop.itemBeingDragged;
            }
        }

        private GameObject itemToBeDeleted;


        public string itemName
        {
            get
            {
                string name = itemToBeDeleted.name;
                string toRemove = "(Clone)";
                string result = name.Replace(toRemove, "");
                return result;
            }
        }



        private void Start()
        {
            textToModify = trashAlertUI.transform.Find("Delete Alert").Find("Question Text").GetComponent<Text>();
            YesBTN = trashAlertUI.transform.Find("Delete Alert").Find("Buttons Container").Find("Button Yes").GetComponent<Button>();
            YesBTN.onClick.AddListener(delegate { DeleteItem(); });
            NoBTN = trashAlertUI.transform.Find("Delete Alert").Find("Buttons Container").Find("Button No").GetComponent<Button>();
            NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
        }


        public void OnDrop(PointerEventData eventData)
        {
            //itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
            if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
            {
                itemToBeDeleted = draggedItem.gameObject;
                StartCoroutine(NotifyBeforeDeletion());
            }
        }

        IEnumerator NotifyBeforeDeletion()
        {
            trashAlertUI.SetActive(true);
            textToModify.text = "Throw away this " + itemName + "?";
            yield return new WaitForSeconds(1f);
        }

        private void CancelDeletion()
        {
            trashAlertUI.SetActive(false);
        }

        private void DeleteItem()
        {
            DestroyImmediate(itemToBeDeleted.gameObject);
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            trashAlertUI.SetActive(false);
        }
    }
}