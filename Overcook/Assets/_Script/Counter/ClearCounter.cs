using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : CounterBase
{
    private GameObject kitchenObj;
    public Material[] originalMats;
    public Material[] approachMats;
    private MeshRenderer meshRenderer;
    [SerializeField] private KitchenObjectSO kitchenData;
    public KitchenObjectSO KitchenData => kitchenData;

    public override void Awake()
    {
        base.Awake();
        EventDispatcher.AddListener<EventDefine.OnPlayerTakeFoodFromClearCounter>(OnPlayerTakeFoodFromClearCounter);
        EventDispatcher.AddListener<EventDefine.OnPlayerGiveFoodToClearCounter>(OnPlayerGiveFoodToClearCounter);
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public override void Start()
    {
        originalMats = meshRenderer.materials;
        approachMats = new Material[originalMats.Length + 1];
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        EventDispatcher.RemoveListener<EventDefine.OnPlayerTakeFoodFromClearCounter>(OnPlayerTakeFoodFromClearCounter);
        EventDispatcher.RemoveListener<EventDefine.OnPlayerGiveFoodToClearCounter>(OnPlayerGiveFoodToClearCounter);
    }

    private void OnPlayerTakeFoodFromClearCounter(EventDefine.OnPlayerTakeFoodFromClearCounter counter)
    {
        if (kitchenData != null)
        {
            kitchenData = null;
            kitchenObj.transform.SetParent(counter.HoldPoint);
            kitchenObj.transform.localPosition = Vector3.zero;
            kitchenObj.transform.localRotation = Quaternion.identity;
        }
    }

    private void OnPlayerGiveFoodToClearCounter(EventDefine.OnPlayerGiveFoodToClearCounter player)
    {
        if (kitchenData == null)
        {
            Debug.Log("Give Food to Clear Counter");
            kitchenData = player.KitchenData;
            Debug.Log("Kitchen Data: " + kitchenData.type);
            if (player.HoldPoint.childCount > 0)
            {
                kitchenObj = player.HoldPoint.GetChild(0).gameObject;
                Debug.Log("Give Food to Clear Counter: " + kitchenObj.name);
            }
            else
            {
                kitchenObj = ObjectPooling.Instance.GetObject(kitchenData.type, Vector3.zero, Quaternion.identity, null);
            }
            kitchenObj.transform.SetParent(spawnPoint);
            kitchenObj.transform.localPosition = Vector3.zero;
            kitchenObj.transform.localRotation = Quaternion.identity;

        }
    }

    public override void HighlightMat()
    {
        meshRenderer.materials = approachMats;
    }

    public override void UnHighlightMat()
    {
        meshRenderer.materials = originalMats;
    }

    public override void AddAproachMaterial()
    {
        for (int i = 0; i < originalMats.Length; i++)
        {
            approachMats[i] = originalMats[i];
        }
        approachMats[originalMats.Length] = pickedMat;
    }
}
