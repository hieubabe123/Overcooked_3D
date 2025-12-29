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

    private void LoadKitchenObjectSO()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>(Resources.LoadAll<KitchenObjectSO>("_Data/ObjectData"));
    }


}
