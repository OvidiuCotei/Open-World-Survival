using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class SetItemID : MonoBehaviour
    {
        public void ChangeName()
        {
            string uniqueID = GenerateUniqueID();
            gameObject.name = gameObject.name + uniqueID;
        }

        private string GenerateUniqueID()
        {
            long timestamp = System.DateTime.Now.Ticks;
            int random = Random.Range(1000, 9999);
            string uniqueID = timestamp.ToString() + random.ToString();
            return uniqueID;
        }
    }
}