using System.Collections.Generic;

namespace OvyKode.Multilanguage
{
    public class LocalizationData
    {
        public List<LocalizationItems> items;
    }

    [System.Serializable]
    public class LocalizationItems
    {
        public string key;
        public string value;
    }
}