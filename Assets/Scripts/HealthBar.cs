using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;
        public Text healthCounter;
        private float currentHealth;
        private float maxHealth;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            currentHealth = PlayerState.Instance.currenthealth;
            maxHealth = PlayerState.Instance.maxHealth;
            float fillValue = currentHealth / maxHealth;
            slider.value = fillValue;
            healthCounter.text = currentHealth + "/" + maxHealth;
        }
    }
}