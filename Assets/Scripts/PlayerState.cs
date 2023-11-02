using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class PlayerState : MonoBehaviour
    {
        public static PlayerState Instance { get; set; }

        // ===== Player Health =====
        public float currenthealth;
        public float maxHealth;
        // ===== Player Calories =====
        public float currentCalories;
        public float maxCalories;
        // ===== Player Hydration =====
        public float currentHydrationPercent;
        public float maxHydrationPercent;
        public bool isHydrationActive;
        private float distanceTraveled = 0;
        private Vector3 lastPosition;
        public GameObject playerBody;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            currenthealth = maxHealth;
            currentCalories = maxCalories;
            currentHydrationPercent = maxHydrationPercent;
            StartCoroutine(DecreaseHydration());
        }

        private void Update()
        {
            distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
            lastPosition = playerBody.transform.position;

            if(distanceTraveled >= 5)
            {
                distanceTraveled = 0;
                currentCalories -= 1;

                if(currentCalories <= 0)
                {
                    currentCalories = 0;
                }
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                currenthealth -= 10;

                if(currenthealth <= 0)
                {
                    currenthealth = 0;
                }
            }
        }

        private IEnumerator DecreaseHydration()
        {
            while (true)
            {
                currentHydrationPercent -= 1;

                if(currentHydrationPercent <= 0)
                {
                    currentHydrationPercent = 0;
                }

                yield return new WaitForSeconds(10f);
            }
        }

        public void SetHealth(float maxHealth)
        {
            currenthealth = maxHealth;
        }

        public void SetCalories(float maxCalories)
        {
            currentCalories = maxCalories;
        }

        public void SetHydration(float maxHydratation)
        {
            currentHydrationPercent = maxHydratation;
        }
    }
}