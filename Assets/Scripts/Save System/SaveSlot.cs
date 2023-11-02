using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class SaveSlot : MonoBehaviour
    {
        private Button button;
        private Text buttonText;
        public int slotNumber;
        public GameObject alertUI;
        public Button buttonYes;
        public Button buttonNo;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonText = transform.Find("Text").GetComponent<Text>();
        }

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                if(SaveManager.Instance.IsSlotEmpty(slotNumber))
                {
                    SaveGameConfirmed();
                }
                else
                {
                    DisplayOverrideWarning();
                }
            });
        }

        private void Update()
        {
            if(SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                buttonText.text = "EMPTY";
            }
            else
            {
                buttonText.text = SaveSystem.GetString("Slot" + slotNumber + "Description");
            }
        }

        public void DisplayOverrideWarning()
        {
            alertUI.SetActive(true);
            buttonYes.onClick.AddListener(() =>
            {
                SaveGameConfirmed();
                alertUI.SetActive(false);
            });

            buttonNo.onClick.AddListener(() =>
            {
                alertUI.SetActive(false);
            });
        }

        private void SaveGameConfirmed()
        {
            SaveManager.Instance.SaveGame(slotNumber);
            DateTime dt = DateTime.Now;
            string time = dt.ToString("yyyy-MM-dd HH:mm:ss");
            string description = "Saved Game " + slotNumber + " | " + time;
            buttonText.text = description;
            SaveSystem.SetString("Slot" + slotNumber + "Description", description);
            SaveManager.Instance.DeselectButton();
        }
    }
}