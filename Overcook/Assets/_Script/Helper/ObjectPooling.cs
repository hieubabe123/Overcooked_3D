using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : SingletonMonoBehaviour<ObjectPooling>
{
    private Dictionary<CounterType, Queue<GameObject>> basePoolDict = new Dictionary<CounterType, Queue<GameObject>>();
    private Dictionary<CounterType, GameObject> prefabDict = new Dictionary<CounterType, GameObject>();
    public int poolSize = 10;

    public void InitPool(CounterType type, GameObject prefab)
    {
        if (basePoolDict.ContainsKey(type))
        {
            return;
        }

        prefabDict[type] = prefab;
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, this.transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        basePoolDict.Add(type, objectPool);
    }

    public GameObject GetObject(CounterType type, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!basePoolDict.ContainsKey(type))
        {
            Debug.LogWarning("Pool for type " + type + " does not exist. Please initialize the pool first.");
            return null;
        }
        Queue<GameObject> objectPool = basePoolDict[type];
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.SetParent(parent);
            return obj;
        }
        else
        {
            if (prefabDict.TryGetValue(type, out GameObject prefab))
            {
                GameObject obj = Instantiate(prefab, position, rotation, this.transform);
                objectPool.Enqueue(obj);
                return obj;
            }
            else
            {
                Debug.LogWarning("Prefab for type " + type + " not found.");
                return null;
            }
        }
    }

    public void ReturnObject(CounterType type, GameObject obj)
    {
        obj.SetActive(false);
        if (basePoolDict.ContainsKey(type))
        {
            basePoolDict[type].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Pool for type " + type + " does not exist. Cannot return object to pool.");
            Destroy(obj);
            return;
        }
    }
}
