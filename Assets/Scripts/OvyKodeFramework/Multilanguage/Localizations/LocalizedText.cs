using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode.Multilanguage
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string localizationKey;
        private Text textComponent;

        private IEnumerator Start()
        {
            while (!LocalizationManager.Instance.isReady)
            {
                yield return null;
            }

            AttributionText();
        }

        public void AttributionText()
        {
            if(textComponent == null)
            {
                textComponent = gameObject.GetComponent<Text>();
            }

            try
            {
                textComponent.text = LocalizationManager.Instance.GetTextForKey(localizationKey);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}