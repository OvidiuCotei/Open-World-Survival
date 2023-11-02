using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class GenerateItemID : MonoBehaviour
    {
        public string ItemID;

        private void Start()
        {
            string uniqueID = GenerateUniqueID();
            gameObject.name = gameObject.name + uniqueID;
            ItemID = gameObject.name;
        }

        private string GenerateUniqueID()
        {
            // Get the current timestamp
            long timestamp = System.DateTime.Now.Ticks;

            // Generate a random number
            int random = Random.Range(1000, 9999);

            // Combine the timestamp and random number to create a unique ID
            string uniqueID = timestamp.ToString() + random.ToString();

            return uniqueID;
        }
    }
}