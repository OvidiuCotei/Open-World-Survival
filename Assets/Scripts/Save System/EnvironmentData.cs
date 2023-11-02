using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    [System.Serializable]
    public class EnvironmentData
    {
        public List<string> pickedupItems;

        public EnvironmentData(List<string> _pickedupItems)
        {
            pickedupItems = _pickedupItems;
        }
    }
}