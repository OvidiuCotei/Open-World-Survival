using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance { get; set; }
        public GameObject menuCanvas;
        public bool isMenuOpen;
        public bool isSaveMenuOpen;

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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen && !isSaveMenuOpen)
            {
                menuCanvas.SetActive(true);
                isMenuOpen = true;
                CursorIsVisible(true);
                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && isMenuOpen && !isSaveMenuOpen)
            {
                menuCanvas.SetActive(false);
                isMenuOpen = false;

                if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
                {
                    CursorIsVisible(false);
                }

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
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