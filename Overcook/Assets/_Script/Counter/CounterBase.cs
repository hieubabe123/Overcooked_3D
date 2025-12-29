using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CounterBase : MonoBehaviour
{
    public enum CounterType
    {
        None,
        Bread,
        Cabbage,
        CabbageSlices,
        CheeseBlock,
        CheeseSlices,
        MeatBurned,
        MeatCooked,
        MeatUncooked,
        Plate,
        Tomato,
        TomatoSlices
    }

    private MeshRenderer meshRenderer;
    private Material[] originalMats;
    private Material[] approachMats;




    [BoxGroup("Material")]
    [SerializeField] protected Material pickedMat;

    [BoxGroup("Position")]
    [SerializeField] protected Transform spawnPoint;

    [BoxGroup("Type")]
    [SerializeField] protected CounterType counterType;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefine.OnPlayerApproachCounter>(OnPlayerApproachCounter);
        EventDispatcher.AddListener<EventDefine.OnPlayerGoAwayFromCounter>(OnPlayerGoAwayFromCounter);
        EventDispatcher.AddListener<EventDefine.OnPlayerInteractCounter>(OnPlayerInteractCounter);
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        originalMats = meshRenderer.materials;
        approachMats = new Material[originalMats.Length + 1];
        AddAproachMaterial();

    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefine.OnPlayerApproachCounter>(OnPlayerApproachCounter);
        EventDispatcher.RemoveListener<EventDefine.OnPlayerGoAwayFromCounter>(OnPlayerGoAwayFromCounter);
        EventDispatcher.RemoveListener<EventDefine.OnPlayerInteractCounter>(OnPlayerInteractCounter);
    }

    private void OnPlayerInteractCounter(EventDefine.OnPlayerInteractCounter evt)
    {
        if (evt.Counter == this)
        {
            Debug.Log("Interacted with " + counterType.ToString() + " counter.");
        }
    }

    private void OnPlayerApproachCounter(EventDefine.OnPlayerApproachCounter evt)
    {
        if (evt.Counter == this)
        {
            HighlightMat();
        }
    }

    private void OnPlayerGoAwayFromCounter(EventDefine.OnPlayerGoAwayFromCounter evt)
    {
        if (evt.Counter == this)
        {
            UnHighlightMat();
        }
    }


    private void HighlightMat()
    {
        meshRenderer.materials = approachMats;
    }

    private void UnHighlightMat()
    {
        meshRenderer.materials = originalMats;
    }

    private void AddAproachMaterial()
    {
        for (int i = 0; i < originalMats.Length; i++)
        {
            approachMats[i] = originalMats[i];
        }
        approachMats[originalMats.Length] = pickedMat;
    }
}
