using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class GlobalState : MonoBehaviour
    {
        public static GlobalState Instance { get; set; }
        public float resourceHealth;
        public float resourceMaxHealth;

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
    }
}