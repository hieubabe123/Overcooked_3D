using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : CounterBase
{
    public Material[] originalMats;
    public Material[] approachMats;
    private MeshRenderer meshRenderer;

    public override void Awake()
    {
        base.Awake();
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
