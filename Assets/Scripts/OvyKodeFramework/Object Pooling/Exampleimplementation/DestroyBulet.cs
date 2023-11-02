using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Pooling;

namespace OvyKode
{
    public class DestroyBulet : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DestroyGo());
        }


        IEnumerator DestroyGo()
        {
            yield return new WaitForSeconds(5);
            ObjectPooler.Instance.DestroyPooling(this.gameObject);
        }
    }
}