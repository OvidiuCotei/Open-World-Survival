using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OvyKode.Singletons;

public class SettingsScreenView : Singleton<SettingsScreenView>
{
    public CanvasGroup settingsScreenCanvasGreup;
    public CanvasGroup videoTabCanvasGroup;
    public CanvasGroup audioTabCanvasGroup;
    public CanvasGroup controlsTabCanvasGroup;
    public CanvasGroup otherTabCanvasGroup;
    public Button buttonSettingsBack;
    [Header("TABS BUTTONS")]
    public Button buttonVideoTab;
    public Button buttonAudioTab;
    public Button buttonControlsTab;
    public Button buttonOtherTab;
    [Header("VIDEO TAB")]
    public Button vSyncButtonLeft;
    public Button vSyncButtonRight;
    public Text vSyncLabel;
    public Button qualityButtonLeft;
    public Button qualityButtonRight;
    public Text qualityLabel;
    public Button windowModelButtonLeft;
    public Button windowModelButtonRight;
    public Text windowModeLabel;
    public Button resolutionButtonLeft;
    public Button resolutionButtonRight;
    public Text resolutionLabel;
    [Header("AUDIO TAB")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambientVolumeSlider;
    public Slider soundFXVolumeSlider;
    public Text labelMasterVolume;
    public Text labelMusicVolume;
    public Text labelAmbientVolume;
    public Text labelSoundFXVolume;
    [Header("OTHER TAB")]
    public Button languageButtonLeft;
    public Button languageButtonRight;
    public Text languageLabel;
}