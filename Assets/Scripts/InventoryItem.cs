using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using OvyKode.Multilanguage;

namespace OvyKode
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Is This Item Treashable")]
        public bool isTrashable;

        [Header("Item Info UI")]
        public string thisNameKey;
        private string thisName;
        public string thisDescriptionKey;
        private string thisDescription;
        public string thisFunctionalotyKey;
        private string thisFunctionality;
        private GameObject itemInfoUI;
        private Text itemInfoUIItemName;
        private Text itemInfoUIItemDescription;
        private Text itemInfoUIItemFunctionality;

        [Header("Consumption")]
        public bool isConsumable;
        public float healthEffect;
        public float caloriesEffect;
        public float hydratationEffect;
        private GameObject itemPendingConsumption;

        [Header("Equipping")]
        public bool isEquippable;
        [HideInInspector] public bool isInsideQuickSlot;
        [HideInInspector] public bool isSelected;
        private GameObject itemPendinfEquipping;

        [Header("Useable")]
        public bool isUseable;

        private void Start()
        {
            itemInfoUI = InventorySystem.Instance.ItemInfoUI;
            itemInfoUIItemName = itemInfoUI.transform.Find("Item Name").GetComponent<Text>();
            itemInfoUIItemDescription = itemInfoUI.transform.Find("Item Description").GetComponent<Text>();
            itemInfoUIItemFunctionality = itemInfoUI.transform.Find("Item Functionality").GetComponent<Text>();

            thisName = LocalizationManager.Instance.GetTextForKey(thisNameKey);
            thisDescription = LocalizationManager.Instance.GetTextForKey(thisDescriptionKey);
            thisFunctionality = LocalizationManager.Instance.GetTextForKey(thisFunctionalotyKey);
        }

        private void Update()
        {
            if (isSelected)
            {
                gameObject.GetComponent<DragDrop>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<DragDrop>().enabled = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            itemInfoUI.SetActive(true);
            itemInfoUIItemName.text = thisName;
            itemInfoUIItemDescription.text = thisDescription;
            itemInfoUIItemFunctionality.text = thisFunctionality;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemInfoUI.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                if(isConsumable)
                {
                    itemPendingConsumption = gameObject;
                    ConsumingFunction(healthEffect, caloriesEffect, hydratationEffect);
                }

                if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
                {
                    EquipSystem.Instance.AddToQuickSlots(gameObject);
                    isInsideQuickSlot = true;
                }

                if(isUseable)
                {
                    ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                    gameObject.SetActive(false);
                    UseItem();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (isConsumable && itemPendingConsumption == gameObject)
                {
                    DestroyImmediate(gameObject);
                    InventorySystem.Instance.ReCalculateList();
                    CraftingSystem.Instance.RefreshNeededItems();
                }
            }
        }

        private void ConsumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
        {
            itemInfoUI.SetActive(false);
            HealthEffectCalculation(healthEffect);
            CaloriesEffectCalculation(caloriesEffect);
            HydrationEffectCalculation(hydrationEffect);
        }

        private static void HealthEffectCalculation(float healthEffect)
        {
            float healthBeforeConsumption = PlayerState.Instance.currenthealth;
            float maxHealth = PlayerState.Instance.maxHealth;

            if (healthEffect != 0)
            {
                if ((healthBeforeConsumption + healthEffect) > maxHealth)
                {
                    PlayerState.Instance.SetHealth(maxHealth);
                }
                else
                {
                    PlayerState.Instance.SetHealth(healthBeforeConsumption + healthEffect);
                }
            }
        }

        private static void CaloriesEffectCalculation(float caloriesEffect)
        {
            float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
            float maxCalories = PlayerState.Instance.maxCalories;

            if (caloriesEffect != 0)
            {
                if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
                {
                    PlayerState.Instance.SetCalories(maxCalories);
                }
                else
                {
                    PlayerState.Instance.SetCalories(caloriesBeforeConsumption + caloriesEffect);
                }
            }
        }

        private static void HydrationEffectCalculation(float hydrationEffect)
        {
            float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
            float maxHydration = PlayerState.Instance.maxHydrationPercent;

            if (hydrationEffect != 0)
            {
                if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
                {
                    PlayerState.Instance.SetHydration(maxHydration);
                }
                else
                {
                    PlayerState.Instance.SetHydration(hydrationBeforeConsumption + hydrationEffect);
                }
            }
        }

        private void UseItem()
        {
            itemInfoUI.SetActive(false);
            InventorySystem.Instance.isOpen = false;
            InventorySystem.Instance.inventoryScreenUI.SetActive(false);
            CraftingSystem.Instance.isOpen = false;
            CraftingSystem.Instance.DisableAllScreens();
            CraftingSystem.Instance.CursorIsVisible(false);
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.enabled = true;

            switch (gameObject.name)
            {
                case "Foundation(Clone)":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                    break;
                case "Foundation":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel"); // For Testing
                    break;
                case "Wall(Clone)":
                    ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                    break;
                case "Wall":
                    ConstructionManager.Instance.ActivateConstructionPlacement("WallModel"); // For Testing
                    break;
                default:
                    break;
            }
        }
    }
}