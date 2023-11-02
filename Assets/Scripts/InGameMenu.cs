using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace OvyKode
{
    public class InGameMenu : MonoBehaviour
    {
        public Button buttonSaveGame;
        public Button buttonQuit;
        public Button closeButton;
        public GameObject mainMenuWrapper;
        public GameObject saveGameDialogWrapper;

        private void Start()
        {
            buttonSaveGame.onClick.AddListener(() =>
            {
                CursorIsVisible(true);
                mainMenuWrapper.SetActive(false);
                saveGameDialogWrapper.SetActive(true);
                MenuManager.Instance.isSaveMenuOpen = true;
            });

            buttonQuit.onClick.AddListener(() =>
            {
                BackToMainMenu();
            });

            closeButton.onClick.AddListener(() =>
            {
                saveGameDialogWrapper.SetActive(false);
                mainMenuWrapper.SetActive(true);
                MenuManager.Instance.isSaveMenuOpen = false;
            });
        }

        private void  BackToMainMenu()
        {
            SceneManager.LoadScene("Persistent Scene");
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