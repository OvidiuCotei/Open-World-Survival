using UnityEngine;

namespace OvyKode.Multilanguage
{
    public static class LocaleHelper
    {
        public static string GetSupportedLanguageCode()
        {
            SystemLanguage lang = Application.systemLanguage;

            switch (lang)
            {
                case SystemLanguage.English:
                    return LocaleApplication.FR;
                case SystemLanguage.Romanian:
                    return LocaleApplication.RO;
                case SystemLanguage.French:
                    return LocaleApplication.FR;
                case SystemLanguage.Spanish:
                    return LocaleApplication.ES;
                default:
                    return GetDefaultSupportedLanguageCode();
            }
        }

        private static string GetDefaultSupportedLanguageCode()
        {
            return LocaleApplication.EN;
        }
    }
}