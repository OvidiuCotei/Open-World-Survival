using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Singletons;

namespace OvyKode.Pooling
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [Header("Prefabs to spawn")]
        public GameObject[] objects;
        [Header("Number of object to spawn")]
        public int[] number;
        public List<GameObject>[] pool;

        private void Awake()
        {
            InstantiateForPooling();
        }

        private void InstantiateForPooling()
        {
            GameObject temp;
            pool = new List<GameObject>[objects.Length];

            for(int count = 0; count < objects.Length; count++)
            {
                pool[count] = new List<GameObject>();

                for(int num = 0; num < number[count]; num++)
                {
                    temp = (GameObject)Instantiate(objects[count]);
                    temp.transform.parent = this.transform;
                    pool[count].Add(temp);
                    temp.SetActive(false);
                }
            }
        }

        public GameObject InstantiatePooling(int ID)
        {
            for(int count = 0; count < pool[ID].Count; count++)
            {
                if(!pool[ID][count].activeSelf)
                {
                    pool[ID][count].SetActive(true);
                    return pool[ID][count];
                }
            }

            pool[ID].Add((GameObject)Instantiate(objects[ID]));
            pool[ID][pool[ID].Count - 1].transform.parent = this.transform;
            return pool[ID][pool[ID].Count - 1];
        }

        public GameObject InstantiatePooling(int ID, Vector3 position, Quaternion rotation)
        {
            for (int count = 0; count < pool[ID].Count; count++)
            {
                if (!pool[ID][count].activeSelf)
                {
                    pool[ID][count].SetActive(true);
                    pool[ID][count].transform.position = position;
                    pool[ID][count].transform.rotation = rotation;
                    return pool[ID][count];
                }
            }

            pool[ID].Add((GameObject)Instantiate(objects[ID]));
            pool[ID][pool[ID].Count -1].transform.position = position;
            pool[ID][pool[ID].Count -1].transform.rotation = rotation;
            pool[ID][pool[ID].Count - 1].transform.parent = this.transform;
            return pool[ID][pool[ID].Count -1];
        }

        public void DestroyPooling(GameObject objectToDestroy)
        {
            objectToDestroy.SetActive(false);
        }
    }
}