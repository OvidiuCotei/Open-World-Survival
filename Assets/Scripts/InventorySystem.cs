using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; set; }

        public GameObject ItemInfoUI;
        public GameObject inventoryScreenUI;
        public GameObject inventoryTrash;
        public Transform inventoryContainer;
        public bool isOpen;

        public List<GameObject> slotList = new List<GameObject>();
        public List<string> itemList = new List<string>();

        private GameObject itemToAdd;
        private GameObject whatSlotToEquip;
        public GameObject pickupAlert;
        public Text pickupName;
        public Image pickupImage;

        public List<string> itemsPickedup;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            CursorIsVisible(false);
        }


        private void Start()
        {
            isOpen = false;
            PopulateSlotList();
        }


        private void Update()
        {
            if (Input.GetKeyDown(InputManager.Instance.InventoryKey) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
            {
                inventoryScreenUI.SetActive(true);
                inventoryTrash.SetActive(true);
                CursorIsVisible(true);
                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
                isOpen = true;

            }
            else if (Input.GetKeyDown(InputManager.Instance.InventoryKey) && isOpen)
            {
                inventoryScreenUI.SetActive(false);
                inventoryTrash.SetActive(false);

                if (!CraftingSystem.Instance.isOpen)
                {
                    CursorIsVisible(false);
                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }

                isOpen = false;
            }
        }

        private void CursorIsVisible(bool isVisible)
        {
            if(isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void PopulateSlotList()
        {
            foreach(Transform child in inventoryContainer)
            {
                if(child.CompareTag("Slot"))
                {
                    slotList.Add(child.gameObject);
                }
            }
        }

        public void AddToInventory(string itemName, string itemNameLanguage)
        {
            if (SaveManager.Instance.isLoading == false)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
            }

            whatSlotToEquip = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);
            Sprite itemIcon = itemToAdd.GetComponent<Image>().sprite;
            TriggerPickupPopUp(itemNameLanguage, itemIcon); // itemName
            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }

        public bool ChechSlotsAvailable(int emptyNeeded)
        {
            int emptySlot = 0;

            foreach(GameObject slot in slotList)
            {
                if(slot.transform.childCount <= 0)
                {
                    emptySlot += 1;
                }
            }

            if (emptySlot >= emptyNeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private GameObject FindNextEmptySlot()
        {
            foreach(GameObject slot in slotList)
            {
                if(slot.transform.childCount == 0)
                {
                    return slot;
                }
            }

            return new GameObject();
        }

        public void RemoveItem(string nameToRemove, int amountToRemove)
        {
            int counter = amountToRemove;

            for (int i = slotList.Count -1; i >= 0; i--)
            {
                if(slotList[i].transform.childCount > 0)
                {
                    if(slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                    {
                        DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                        counter -= 1;
                    }
                }
            }

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }

        public void ReCalculateList()
        {
            itemList.Clear();

            foreach (GameObject slot in slotList)
            {
                if(slot.transform.childCount > 0)
                {
                    string name = slot.transform.GetChild(0).name;
                    string str2 = "(Clone)";
                    string result = name.Replace(str2, "");
                    itemList.Add(result);
                }
            }
        }

        private void TriggerPickupPopUp(string itemName, Sprite itemSprite)
        {
            pickupName.text = string.Empty;
            pickupImage.sprite = null;
            pickupAlert.SetActive(true);

            pickupName.text = itemName;
            pickupImage.type = Image.Type.Simple;
            pickupImage.sprite = itemSprite;
            StartCoroutine(PickupPopUpClose());
        }

        private IEnumerator PickupPopUpClose()
        {
            yield return new WaitForSeconds(2f);
            pickupAlert.SetActive(false);
        }
    }
}