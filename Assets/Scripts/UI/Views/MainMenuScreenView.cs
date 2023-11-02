using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OvyKode.Singletons;

namespace OvyKode
{
    public class MainMenuScreenView : Singleton<MainMenuScreenView>
    {
        public Button buttonContinue;
        public Button buttonPlay;
        public Button buttonSettings;
        public Button buttonQuit;
        public Button buttonDialogNo;
        public Button buttonDialogYes;
        public CanvasGroup mainMenuScreenCanvasGroup;
        public CanvasGroup quitDialogWrapperCanvasGroup;
        public GameObject quitDialogWrapper;
        public GameObject quitDialogContainer;
        public GameObject loadDialogWrapper;
    }
}