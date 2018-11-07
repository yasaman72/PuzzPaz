using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;


    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool,gameObject.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }


        if (objectToPool.tag == tag)
        {
            if (shouldExpand)
            {
                GameObject obj = Instantiate(objectToPool,gameObject.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }

        }
        return null;

    }
}
