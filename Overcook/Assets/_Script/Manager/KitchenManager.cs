using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KitchenManager : SingletonDontDestroy<KitchenManager>
{
    public List<KitchenObjectSO> kitchenObjectSOList;

    void Awake()
    {
        LoadKitchenObjectSO();
    }

    void Start()
    {
        InitKitchenObjectPool();
    }

    private void LoadKitchenObjectSO()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>(Resources.LoadAll<KitchenObjectSO>("_Data/ObjectData"));
    }

    private void InitKitchenObjectPool()
    {
        foreach (var kitchenObj in kitchenObjectSOList)
        {
            if (kitchenObj.type != CounterType.None && kitchenObj.objectPrefab != null)
            {
                ObjectPooling.Instance.InitPool(kitchenObj.type, kitchenObj.objectPrefab);
            }
        }
    }

    public KitchenObjectSO GetKitchenObjectData(CounterType counterType)
    {
        return kitchenObjectSOList.Find(k => k.type == counterType);
    }


}
