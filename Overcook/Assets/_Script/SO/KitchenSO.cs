using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "KitchenData", menuName = "ScriptableObjects/Kitchen")]
public class KitchenSO : ScriptableObject
{
    public List<KitchenObjectSO> KitchenDataList = new List<KitchenObjectSO>();
}
