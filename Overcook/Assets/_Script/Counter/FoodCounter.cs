using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCounter : CounterBase
{
    private MeshRenderer[] meshRenderers;
    private Dictionary<MeshRenderer, Material[]> originalMatsDict = new Dictionary<MeshRenderer, Material[]>();

    public override void Awake()
    {
        base.Awake();
        EventDispatcher.AddListener<EventDefine.OnPlayerInteractToFoodCounter>(OnPlayerInteractCounter);
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public override void Start()
    {
        foreach (var render in meshRenderers)
        {
            originalMatsDict[render] = render.sharedMaterials;
        }
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        EventDispatcher.RemoveListener<EventDefine.OnPlayerInteractToFoodCounter>(OnPlayerInteractCounter);

    }

    private void OnPlayerInteractCounter(EventDefine.OnPlayerInteractToFoodCounter foodCounter)
    {
        if (foodCounter.Counter == this)
        {
            if (counterType != CounterType.None)
            {
                GameObject obj = ObjectPooling.Instance.GetObject(counterType, foodCounter.HoldPoint.position, foodCounter.HoldPoint.rotation, foodCounter.HoldPoint);
            }
        }
    }

    public override void HighlightMat()
    {
        foreach (var render in meshRenderers)
        {
            List<Material> mats = new List<Material>(originalMatsDict[render]);
            mats.Add(pickedMat);
            render.materials = mats.ToArray();
        }
    }

    public override void UnHighlightMat()
    {
        foreach (var render in meshRenderers)
        {
            render.materials = originalMatsDict[render];
        }
    }
}
