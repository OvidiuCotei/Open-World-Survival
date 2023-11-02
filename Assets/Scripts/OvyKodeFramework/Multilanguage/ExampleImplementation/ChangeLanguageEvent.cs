using UnityEngine;
using OvyKode.Multilanguage;

public class ChangeLanguageEvent : MonoBehaviour
{
    public void OnClickChangeLanguageRuntime(string lang)
    {
        LocalizationManager.Instance.ChangeLanguage(lang);
    }
}