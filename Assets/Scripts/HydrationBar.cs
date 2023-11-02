using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class HydrationBar : MonoBehaviour
    {
        private Slider slider;
        public Text hydratationCounter;
        private float currentHydratation;
        private float maxHydratation;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            currentHydratation = PlayerState.Instance.currentHydrationPercent;
            maxHydratation = PlayerState.Instance.maxHydrationPercent;
            float fillValue = currentHydratation / maxHydratation;
            slider.value = fillValue;
            hydratationCounter.text = currentHydratation + "%" ;
        }
    }
}