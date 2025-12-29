using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenData", menuName = "ScriptableObjects/KitchenData")]
public class KitchenObjectSO : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public GameObject objectPrefab;
}
