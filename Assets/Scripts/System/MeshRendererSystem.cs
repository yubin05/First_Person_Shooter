using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshRendererSystem : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private ShadowCastingMode shadowCastingMode;

    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.shadowCastingMode = shadowCastingMode;
        }
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            skinnedMeshRenderer.shadowCastingMode = shadowCastingMode;
        }
    }
}
