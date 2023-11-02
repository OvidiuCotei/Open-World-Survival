using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class LoadSlot : MonoBehaviour
    {
        private Button button;
        private Text buttonText;
        public int slotNumber;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonText = transform.Find("Text").GetComponent<Text>();
        }

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                if (SaveManager.Instance.IsSlotEmpty(slotNumber) == false)
                {
                    LoadingScreenController.Instance.StartGame(true, 1);
                    SaveManager.Instance.DeselectButton();
                }
                else
                {
                    // If empty do nothing
                }
            });
        }

        private void Update()
        {
            if(SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                buttonText.text = string.Empty;
            }
            else
            {
                buttonText.text = SaveSystem.GetString("Slot" + slotNumber + "Description");
            }
        }
    }
}