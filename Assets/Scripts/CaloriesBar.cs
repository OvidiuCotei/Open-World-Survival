using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class CaloriesBar : MonoBehaviour
    {
        private Slider slider;
        public Text caloriesCounter;
        private float currentCalories;
        private float maxCalories;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            currentCalories = PlayerState.Instance.currentCalories;
            maxCalories = PlayerState.Instance.maxCalories;
            float fillValue = currentCalories / maxCalories;
            slider.value = fillValue;
            caloriesCounter.text = currentCalories + "/" + maxCalories;
        }
    }
}