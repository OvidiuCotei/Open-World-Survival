using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using OvyKode.Multilanguage;

namespace OvyKode
{
    public class SettingsScreenController : MonoBehaviour
    {
        public AudioMixer theMixer;
        public List<ResItem> resolutions = new List<ResItem>();
        //private Tween fadeTween;
        public Color buttonsTabsHoverColor;
        private int selectedResolution;
        private int selectedQuality = 0;
        private int selectedLanguage = 0;

        private void Start()
        {
            AddButtonsEvents();
            SetDeafaultVideoSettings();
            SetDefaultSoundSettings();
        }

        private void AddButtonsEvents()
        {
            SettingsScreenView.Instance.buttonSettingsBack.onClick.AddListener(() =>
            {
                FadeInMenuScreen(1f);
            });

            SettingsScreenView.Instance.buttonVideoTab.onClick.AddListener(() =>
            {
                OnClickButtonVideoTab();
            });

            SettingsScreenView.Instance.buttonAudioTab.onClick.AddListener(() =>
            {
                OnClickButtonAudioTab();
            });

            SettingsScreenView.Instance.buttonControlsTab.onClick.AddListener(() =>
            {
                OnClickButtonControlsTab();
            });

            SettingsScreenView.Instance.buttonOtherTab.onClick.AddListener(() =>
            {
                OnClickButtonOtherTab();
            });

            SettingsScreenView.Instance.windowModelButtonLeft.onClick.AddListener(() =>
            {
                OnClickButtonLeftWindowMode();
            });

            SettingsScreenView.Instance.windowModelButtonRight.onClick.AddListener(() =>
            {
                OnClickButtonRighttWindowMode();
            });

            SettingsScreenView.Instance.vSyncButtonLeft.onClick.AddListener(() =>
            {
                OnClickButtonLeftVSync();
            });


            SettingsScreenView.Instance.vSyncButtonRight.onClick.AddListener(() =>
            {
                OnClickButtonRightVSync();
            });

            SettingsScreenView.Instance.resolutionButtonLeft.onClick.AddListener(() => {
                OnClickButtonLeftResolution();
            });

            SettingsScreenView.Instance.resolutionButtonRight.onClick.AddListener(() =>
            {
                OnClickButtonRightResolution();
            });

            SettingsScreenView.Instance.qualityButtonLeft.onClick.AddListener(() =>
            {
                OnClickButtonLeftQuality();
            });

            SettingsScreenView.Instance.qualityButtonRight.onClick.AddListener(() =>
            {
                OnClickButtonRightQuality();
            });

            SettingsScreenView.Instance.masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
            SettingsScreenView.Instance.musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            SettingsScreenView.Instance.ambientVolumeSlider.onValueChanged.AddListener(delegate { SetAmbientVolume(); });
            SettingsScreenView.Instance.soundFXVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });

            SettingsScreenView.Instance.languageButtonLeft.onClick.AddListener(() =>
            {
                OnClickButtonLeftLanguage();
            });


            SettingsScreenView.Instance.languageButtonRight.onClick.AddListener(() =>
            {
                OnClickButtonRightLanguage();
            });
        }

        private void SetDeafaultVideoSettings()
        {
            if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                SettingsScreenView.Instance.windowModeLabel.text = "BORDERLESS";
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                SettingsScreenView.Instance.windowModeLabel.text = "FULLSCREEN";
            }

            var vSyncValue = SaveSystem.GetInt("VSync");

            if (vSyncValue == 0)
            {
                QualitySettings.vSyncCount = 0;
                SettingsScreenView.Instance.vSyncLabel.text = "OFF";
            }
            else
            {
                QualitySettings.vSyncCount = 1;
                SettingsScreenView.Instance.vSyncLabel.text = "ON";
            }

            bool foundRes = false;

            for (int i = 0; i < resolutions.Count; i++)
            {
                if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
                {
                    foundRes = true;
                    selectedResolution = i;
                    UpdateResolution();
                }
            }

            if(!foundRes)
            {
                ResItem newRes = new ResItem();
                newRes.horizontal = Screen.width;
                newRes.vertical = Screen.height;
                resolutions.Add(newRes);
                selectedResolution = resolutions.Count - 1;
                UpdateResolution();
            }

            int quality = SaveSystem.GetInt("QualityLevel");
            selectedQuality = quality;
            QualitySettings.SetQualityLevel(quality);
            UpdateQualityLabel();

            SettingsScreenView.Instance.masterVolumeSlider.minValue = -80;
            SettingsScreenView.Instance.masterVolumeSlider.maxValue = 20;

            SettingsScreenView.Instance.musicVolumeSlider.minValue = -80;
            SettingsScreenView.Instance.musicVolumeSlider.maxValue = 20;

            SettingsScreenView.Instance.ambientVolumeSlider.minValue = -80;
            SettingsScreenView.Instance.ambientVolumeSlider.maxValue = 20;

            SettingsScreenView.Instance.soundFXVolumeSlider.minValue = -80;
            SettingsScreenView.Instance.soundFXVolumeSlider.maxValue = 20;

            int lang = SaveSystem.GetInt("Language");
            Debug.Log("Ce limba am incarcat: " + lang);
            selectedLanguage = lang;
            UpdateLanguage();
            UpdateLanguageLabel();

        }

        private void SetDefaultSoundSettings()
        {
            float masterVol = SaveSystem.GetFloat("MasterVol");
            theMixer.SetFloat("MasterVol", masterVol);

            float musicVol = SaveSystem.GetFloat("MusicVol");
            theMixer.SetFloat("MusicVol", musicVol);

            float ambientVol = SaveSystem.GetFloat("AmbientVol");
            theMixer.SetFloat("AmbientVol", ambientVol);

            float soundFXVol = SaveSystem.GetFloat("SFXVol");
            theMixer.SetFloat("SFXVol", soundFXVol);


            float _masterVol = 0f;
            theMixer.GetFloat("MasterVol", out _masterVol);
            SettingsScreenView.Instance.masterVolumeSlider.value = _masterVol;
            SettingsScreenView.Instance.labelMasterVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.masterVolumeSlider.value + 80).ToString() + "%";

            float _musicVol = 0f;
            theMixer.GetFloat("MusicVol", out _musicVol);
            SettingsScreenView.Instance.musicVolumeSlider.value = _musicVol;
            SettingsScreenView.Instance.labelMusicVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.musicVolumeSlider.value + 80).ToString() + "%";

            float _ambientVol = 0f;
            theMixer.GetFloat("AmbientVol", out _ambientVol);
            SettingsScreenView.Instance.ambientVolumeSlider.value = _ambientVol;
            SettingsScreenView.Instance.labelAmbientVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.ambientVolumeSlider.value + 80).ToString() + "%";

            float _soundFXVol = 0f;
            theMixer.GetFloat("SFXVol", out _soundFXVol);
            SettingsScreenView.Instance.soundFXVolumeSlider.value = _soundFXVol;
            SettingsScreenView.Instance.labelSoundFXVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.soundFXVolumeSlider.value + 80).ToString() + "%";
        }

        private void OnClickButtonLeftResolution()
        {
            selectedResolution--;

            if(selectedResolution <= 0)
            {
                selectedResolution = 0;
            }

            UpdateResolution();
        }

        private void OnClickButtonRightResolution()
        {
            selectedResolution++;

            if(selectedResolution >= resolutions.Count -1)
            {
                selectedResolution = resolutions.Count - 1;
            }

            UpdateResolution();
        }

        private void UpdateResolution()
        {
            SettingsScreenView.Instance.resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + "X" + resolutions[selectedResolution].vertical.ToString();
            Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, Screen.fullScreen);
        }

        private void OnClickButtonLeftQuality()
        {
            selectedQuality--;

            if (selectedQuality <= 0)
            {
                selectedQuality = 0;
            }

            SaveSystem.SetInt("QualityLevel", selectedQuality);
            QualitySettings.SetQualityLevel(selectedQuality);
            UpdateQualityLabel();
        }

        private void OnClickButtonRightQuality()
        {
            selectedQuality++;

            if (selectedQuality >= 5)
            {
                selectedQuality = 5;
            }

            SaveSystem.SetInt("QualityLevel", selectedQuality);
            QualitySettings.SetQualityLevel(selectedQuality);
            UpdateQualityLabel();
        }

        private void UpdateQualityLabel()
        {
            switch (selectedQuality)
            {
                case 0:
                    SettingsScreenView.Instance.qualityLabel.text = "VERY LOW";
                    break;
                case 1:
                    SettingsScreenView.Instance.qualityLabel.text = "LOW";
                    break;
                case 2:
                    SettingsScreenView.Instance.qualityLabel.text = "MEDIUM";
                    break;
                case 3:
                    SettingsScreenView.Instance.qualityLabel.text = "HIGH";
                    break;
                case 4:
                    SettingsScreenView.Instance.qualityLabel.text = "VERY HIGH";
                    break;
                case 5:
                    SettingsScreenView.Instance.qualityLabel.text = "ULTRA";
                    break;
            }
        }

        private void OnClickButtonLeftWindowMode()
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            SettingsScreenView.Instance.windowModeLabel.text = "BORDERLESS";
        }

        private void OnClickButtonRighttWindowMode()
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            SettingsScreenView.Instance.windowModeLabel.text = "FULLSCREEN";
        }

        private void OnClickButtonLeftVSync()
        {
            SettingsScreenView.Instance.vSyncLabel.text = "OFF";
            QualitySettings.vSyncCount = 0;
            SaveSystem.SetInt("VSync", 0);
        }

        private void OnClickButtonRightVSync()
        {
            SettingsScreenView.Instance.vSyncLabel.text = "ON";
            QualitySettings.vSyncCount = 1;
            SaveSystem.SetInt("VSync", 1);
        }

        private void OnClickButtonVideoTab()
        {
            ButtonsTabsDeselectHover();
            SettingsScreenView.Instance.buttonVideoTab.image.color = buttonsTabsHoverColor;
            SettingsScreenView.Instance.buttonVideoTab.GetComponentInChildren<Text>().color = Color.white;
            FadeInVideoTab(0.2f);
        }

        private void OnClickButtonAudioTab()
        {
            ButtonsTabsDeselectHover();
            SettingsScreenView.Instance.buttonAudioTab.image.color = buttonsTabsHoverColor;
            SettingsScreenView.Instance.buttonAudioTab.GetComponentInChildren<Text>().color = Color.white;
            FadeInAudioTab(0.2f);
        }

        private void OnClickButtonControlsTab()
        {
            ButtonsTabsDeselectHover();
            SettingsScreenView.Instance.buttonControlsTab.image.color = buttonsTabsHoverColor;
            SettingsScreenView.Instance.buttonControlsTab.GetComponentInChildren<Text>().color = Color.white;
            FadeInControlsTab(0.2f);
        }

        private void OnClickButtonOtherTab()
        {
            ButtonsTabsDeselectHover();
            SettingsScreenView.Instance.buttonOtherTab.image.color = buttonsTabsHoverColor;
            SettingsScreenView.Instance.buttonOtherTab.GetComponentInChildren<Text>().color = Color.white;
            FadeInOtherTab(0.2f);
        }

        private void FadeInMenuScreen(float duration)
        {
            Fade(MainMenuScreenView.Instance.mainMenuScreenCanvasGroup, 1f, duration, () => {           
                MainMenuScreenView.Instance.mainMenuScreenCanvasGroup.interactable = true;
                MainMenuScreenView.Instance.mainMenuScreenCanvasGroup.blocksRaycasts = true;
                FadeOutSettingsScreen(1f);
                FadeOutVideoTab(1f);
                FadeOutAudioTab(1f);
                FadeOutControlsTab(1f);
                FadeOutOtherTab(1f);
                ButtonsTabsDeselectHover();
                SettingsScreenView.Instance.buttonVideoTab.image.color = buttonsTabsHoverColor;
                SettingsScreenView.Instance.buttonVideoTab.GetComponentInChildren<Text>().color = Color.white;
            });
        }

        private void FadeOutSettingsScreen(float duration)
        {
            Fade(SettingsScreenView.Instance.settingsScreenCanvasGreup, 0f, duration, () => {
                SettingsScreenView.Instance.settingsScreenCanvasGreup.interactable = false;
                SettingsScreenView.Instance.settingsScreenCanvasGreup.blocksRaycasts = false;
            });
        }

        private void FadeInVideoTab(float duration)
        {
            Fade(SettingsScreenView.Instance.videoTabCanvasGroup, 1f, duration, () => {
                SettingsScreenView.Instance.videoTabCanvasGroup.interactable = true;
                SettingsScreenView.Instance.videoTabCanvasGroup.blocksRaycasts = true;
                FadeOutAudioTab(0.2f);
                FadeOutControlsTab(0.2f);
                FadeOutOtherTab(0.2f);
            });
        }

        private void FadeOutVideoTab(float duration)
        {
            Fade(SettingsScreenView.Instance.videoTabCanvasGroup, 0f, duration, () => {
                SettingsScreenView.Instance.videoTabCanvasGroup.interactable = false;
                SettingsScreenView.Instance.videoTabCanvasGroup.blocksRaycasts = false;
            });
        }

        private void FadeInAudioTab(float duration)
        {
            Fade(SettingsScreenView.Instance.audioTabCanvasGroup, 1f, duration, () => {
                SettingsScreenView.Instance.audioTabCanvasGroup.interactable = true;
                SettingsScreenView.Instance.audioTabCanvasGroup.blocksRaycasts = true;
                FadeOutVideoTab(0.2f);
                FadeOutControlsTab(0.2f);
                FadeOutOtherTab(0.2f);
            });
        }

        private void FadeOutAudioTab(float duration)
        {
            Fade(SettingsScreenView.Instance.audioTabCanvasGroup, 0f, duration, () => {
                SettingsScreenView.Instance.audioTabCanvasGroup.interactable = false;
                SettingsScreenView.Instance.audioTabCanvasGroup.blocksRaycasts = false;
            });
        }

        private void FadeInControlsTab(float duration)
        {
            Fade(SettingsScreenView.Instance.controlsTabCanvasGroup, 1f, duration, () => {
                SettingsScreenView.Instance.controlsTabCanvasGroup.interactable = true;
                SettingsScreenView.Instance.controlsTabCanvasGroup.blocksRaycasts = true;
                FadeOutVideoTab(0.2f);
                FadeOutAudioTab(0.2f);
                FadeOutOtherTab(0.2f);
            });
        }

        private void FadeOutControlsTab(float duration)
        {
            Fade(SettingsScreenView.Instance.controlsTabCanvasGroup, 0f, duration, () => {
                SettingsScreenView.Instance.controlsTabCanvasGroup.interactable = false;
                SettingsScreenView.Instance.controlsTabCanvasGroup.blocksRaycasts = false;
            });
        }

        private void FadeInOtherTab(float duration)
        {
            Fade(SettingsScreenView.Instance.otherTabCanvasGroup, 1f, duration, () => {
                SettingsScreenView.Instance.otherTabCanvasGroup.interactable = true;
                SettingsScreenView.Instance.otherTabCanvasGroup.blocksRaycasts = true;
                FadeOutVideoTab(0.2f);
                FadeOutAudioTab(0.2f);
                FadeOutControlsTab(0.2f);
            });
        }

        private void FadeOutOtherTab(float duration)
        {
            Fade(SettingsScreenView.Instance.otherTabCanvasGroup, 0f, duration, () => {
                SettingsScreenView.Instance.otherTabCanvasGroup.interactable = false;
                SettingsScreenView.Instance.otherTabCanvasGroup.blocksRaycasts = false;
            });
        }

        private void Fade(CanvasGroup canvasGroup, float endValue, float duration, TweenCallback onEnd)
        {
            //if (fadeTween != null)
            //{
            //    fadeTween.Kill(false);
            //}

            //fadeTween = canvasGroup.DOFade(endValue, duration);
            //fadeTween.onComplete += onEnd;

            canvasGroup.DOFade(endValue, duration).onComplete += onEnd;
        }

        private void ButtonsTabsDeselectHover()
        {
            SettingsScreenView.Instance.buttonVideoTab.image.color = Color.white;
            SettingsScreenView.Instance.buttonVideoTab.GetComponentInChildren<Text>().color = Color.black;

            SettingsScreenView.Instance.buttonAudioTab.image.color = Color.white;
            SettingsScreenView.Instance.buttonAudioTab.GetComponentInChildren<Text>().color = Color.black;

            SettingsScreenView.Instance.buttonControlsTab.image.color = Color.white;
            SettingsScreenView.Instance.buttonControlsTab.GetComponentInChildren<Text>().color = Color.black;

            SettingsScreenView.Instance.buttonOtherTab.image.color = Color.white;
            SettingsScreenView.Instance.buttonOtherTab.GetComponentInChildren<Text>().color = Color.black;
        }

        private void SetMasterVolume()
        {
            SettingsScreenView.Instance.labelMasterVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.masterVolumeSlider.value + 80).ToString() + "%";
            theMixer.SetFloat("MasterVol", SettingsScreenView.Instance.masterVolumeSlider.value);
            SaveSystem.SetFloat("MasterVol", SettingsScreenView.Instance.masterVolumeSlider.value);
        }

        private void SetMusicVolume()
        {
            SettingsScreenView.Instance.labelMusicVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.musicVolumeSlider.value + 80).ToString() + "%";
            theMixer.SetFloat("MusicVol", SettingsScreenView.Instance.musicVolumeSlider.value);
            SaveSystem.SetFloat("MusicVol", SettingsScreenView.Instance.musicVolumeSlider.value);
        }

        private void SetAmbientVolume()
        {
            SettingsScreenView.Instance.labelAmbientVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.ambientVolumeSlider.value + 80).ToString() + "%";
            theMixer.SetFloat("AmbientVol", SettingsScreenView.Instance.ambientVolumeSlider.value);
            SaveSystem.SetFloat("AmbientVol", SettingsScreenView.Instance.ambientVolumeSlider.value);
        }

        private void SetSFXVolume()
        {
            SettingsScreenView.Instance.labelSoundFXVolume.text = Mathf.RoundToInt(SettingsScreenView.Instance.soundFXVolumeSlider.value + 80).ToString() + "%";
            theMixer.SetFloat("SFXVol", SettingsScreenView.Instance.soundFXVolumeSlider.value);
            SaveSystem.SetFloat("SFXVol", SettingsScreenView.Instance.soundFXVolumeSlider.value);
        }

        private void UpdateLanguage()
        {
            switch (selectedLanguage)
            {
                case 0:
                    LocalizationManager.Instance.ChangeLanguage("EN");
                    break;
                case 1:
                    LocalizationManager.Instance.ChangeLanguage("RO");
                    break;
                case 2:
                    LocalizationManager.Instance.ChangeLanguage("ES");
                    Debug.Log("SELECTED LANGUAGE: " + selectedLanguage);
                    break;
                case 3:
                    LocalizationManager.Instance.ChangeLanguage("FR");
                    break;
            }
        }

        private void OnClickButtonLeftLanguage()
        {
            selectedLanguage--;

            if (selectedLanguage <= 0)
            {
                selectedLanguage = 0;
            }

            SaveSystem.SetInt("Language", selectedLanguage);
            UpdateLanguage();
            UpdateLanguageLabel();
        }

        private void OnClickButtonRightLanguage()
        {
            selectedLanguage++;

            if (selectedLanguage >= 4)
            {
                selectedLanguage = 4;
            }

            SaveSystem.SetInt("Language", selectedLanguage);
            Debug.Log("Limba aleasa: " + selectedLanguage);
            UpdateLanguage();
            UpdateLanguageLabel();
        }

        private void UpdateLanguageLabel()
        {
            switch (selectedLanguage)
            {
                case 0:
                    SettingsScreenView.Instance.languageLabel.text = "ENGLISH";
                    break;
                case 1:
                    SettingsScreenView.Instance.languageLabel.text = "ROMANIAN";
                    break;
                case 2:
                    SettingsScreenView.Instance.languageLabel.text = "SPANISH";
                    break;
                case 3:
                    SettingsScreenView.Instance.languageLabel.text = "FRENCH";
                    break;
            }
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;
    public int vertical;
}