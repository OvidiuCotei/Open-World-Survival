using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class DialogSystem : MonoBehaviour
    {
        public static DialogSystem Instance { get; set; }

        public Text dialogText;
        public Button option1Btn;
        public Button option2Btn;
        public GameObject dialogUI;
        public bool dialogUIActive;

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
        }

        public void OpenDialogUI()
        {
            dialogUIActive = true;
            dialogUI.SetActive(true);
            CursorIsVisible(true);
        }

        public void CloseDialogUI()
        {
            dialogUIActive = false;
            dialogUI.SetActive(false);
            CursorIsVisible(false);
        }

        private void CursorIsVisible(bool isVisible)
        {
            if (isVisible)
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
    }
}