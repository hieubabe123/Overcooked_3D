using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
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

public class CounterBase : MonoBehaviour
{
    [BoxGroup("Material")]
    [SerializeField] protected Material pickedMat;

    [BoxGroup("Position")]
    [SerializeField] protected Transform spawnPoint;

    [BoxGroup("Type")]
    [SerializeField] protected CounterType counterType;
    public CounterType CounterType => counterType;

    public virtual void Awake()
    {
        EventDispatcher.AddListener<EventDefine.OnPlayerApproachCounter>(OnPlayerApproachCounter);
        EventDispatcher.AddListener<EventDefine.OnPlayerGoAwayFromCounter>(OnPlayerGoAwayFromCounter);
    }

    public virtual void Start()
    {
        AddAproachMaterial();
    }
    public virtual void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefine.OnPlayerApproachCounter>(OnPlayerApproachCounter);
        EventDispatcher.RemoveListener<EventDefine.OnPlayerGoAwayFromCounter>(OnPlayerGoAwayFromCounter);
    }


    public virtual void OnPlayerApproachCounter(EventDefine.OnPlayerApproachCounter evt)
    {
        if (evt.Counter == this)
        {
            HighlightMat();
        }
    }

    public virtual void OnPlayerGoAwayFromCounter(EventDefine.OnPlayerGoAwayFromCounter evt)
    {
        if (evt.Counter == this)
        {
            UnHighlightMat();
        }
    }


    public virtual void HighlightMat()
    {
        Debug.Log("Highlight Material");
    }

    public virtual void UnHighlightMat()
    {

    }

    public virtual void AddAproachMaterial()
    {
    }
}
