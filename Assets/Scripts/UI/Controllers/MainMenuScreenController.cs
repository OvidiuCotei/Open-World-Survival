using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OvyKode
{
    public class MainMenuScreenController : MonoBehaviour
    {
        //private Tween fadeTween;

        private void Start()
        {
            AddButtonsEvents();
        }

        private void AddButtonsEvents()
        {
            MainMenuScreenView.Instance.buttonContinue.onClick.AddListener(() =>
            {
                OnClickButtonContinue();
            });

            MainMenuScreenView.Instance.buttonPlay.onClick.AddListener(() =>
            {
                OnClickButtonPlay();
            });

            MainMenuScreenView.Instance.buttonSettings.onClick.AddListener(() =>
            {
                OnClickButtonSettings();
            });

            MainMenuScreenView.Instance.buttonQuit.onClick.AddListener(() =>
            {
                OnClickButtonQuit();
            });

            MainMenuScreenView.Instance.buttonDialogNo.onClick.AddListener(() =>
            {
                OnClickButtonDialogNo();
            });

            MainMenuScreenView.Instance.buttonDialogYes.onClick.AddListener(() =>
            {
                OnClickButtonDialogYes();
            });
        }

        private void OnClickButtonContinue()
        {
            MainMenuScreenView.Instance.loadDialogWrapper.SetActive(true);
        }

        private void OnClickButtonPlay()
        {
            LoadingScreenController.Instance.StartGame(false, 0);
        }

        private void OnClickButtonSettings()
        {
            FadeInSettingsScreen(1f);
        }

        private void OnClickButtonQuit()
        {
            FadeInDialogQuit(1f);
        }

        private void OnClickButtonDialogNo()
        {
            FadeOutDialogQuit(1f);
        }

        private void OnClickButtonDialogYes()
        {
            Application.Quit();
        }

        private void FadeInSettingsScreen(float duration)
        {
            Fade(SettingsScreenView.Instance.settingsScreenCanvasGreup, 1f, duration, () => {
                SettingsScreenView.Instance.settingsScreenCanvasGreup.interactable = true;
                SettingsScreenView.Instance.settingsScreenCanvasGreup.blocksRaycasts = true;
                FadeOutMenuScreen(0.5f);
            });
        }

        private void FadeOutMenuScreen(float duration)
        {
            Fade(MainMenuScreenView.Instance.mainMenuScreenCanvasGroup, 0f, duration, () =>
            {
                MainMenuScreenView.Instance.mainMenuScreenCanvasGroup.interactable = false;
                MainMenuScreenView.Instance.mainMenuScreenCanvasGroup.blocksRaycasts = false;
                FadeInVideoTab(1f);
            });
        }

        private void FadeInVideoTab(float duration)
        {
            Fade(SettingsScreenView.Instance.videoTabCanvasGroup, 1f, duration, () => {
                SettingsScreenView.Instance.videoTabCanvasGroup.interactable = true;
                SettingsScreenView.Instance.videoTabCanvasGroup.blocksRaycasts = true;
            });
        }

        private void FadeInDialogQuit(float duration)
        {
            Fade(MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup, 1f, duration, () =>
            {
                MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup.interactable = true;
                MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup.blocksRaycasts = true;

                //MainMenuScreenView.Instance.quitDialogContainer.SetActive(true);
                MainMenuScreenView.Instance.quitDialogContainer.transform.DOShakeScale(1f, 0.5f, 1, 45, true);
            });
        }

        private void FadeOutDialogQuit(float duration)
        {
            MainMenuScreenView.Instance.quitDialogContainer.transform.DOShakeScale(1f, 0.5f, 1, 45, true).OnComplete(() => {
                //MainMenuScreenView.Instance.quitDialogContainer.SetActive(false);

                Fade(MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup, 0f, duration, () =>
                {
                    MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup.interactable = false;
                    MainMenuScreenView.Instance.quitDialogWrapperCanvasGroup.blocksRaycasts = false;
                });
            });
        }

        private void Fade(CanvasGroup canvasGroup, float endValue, float duration, TweenCallback onEnd)
        {
            //if(fadeTween != null)
            //{
            //    fadeTween.Kill(false);
            //}

            //fadeTween = canvasGroup.DOFade(endValue, duration);
            //fadeTween.onComplete += onEnd;

            canvasGroup.DOFade(endValue, duration).onComplete += onEnd;
        }
    }
}