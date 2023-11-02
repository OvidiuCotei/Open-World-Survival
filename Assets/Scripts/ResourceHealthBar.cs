using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class ResourceHealthBar : MonoBehaviour
    {
        private Slider slider;
        private float currentHealth;
        private float maxHealth;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            currentHealth = GlobalState.Instance.resourceHealth;
            maxHealth = GlobalState.Instance.resourceMaxHealth;
            float fillValue = currentHealth / maxHealth;
            slider.value = fillValue;
        }
    }
}