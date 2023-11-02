using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Pooling;

namespace OvyKode
{
    public class BulletController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                FireCub();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                FireSphere();
            }
        }

        private void FireCub()
        {
            ObjectPooler.Instance.InstantiatePooling(0, new Vector3(0f, 0, 0f), Quaternion.identity);
        }

        private void FireSphere()
        {
            ObjectPooler.Instance.InstantiatePooling(1, new Vector3(0f, 0, 0f), Quaternion.identity);
        }
    }
}