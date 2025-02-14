using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponObject : EntityObject
{
    protected MeshRenderer[] meshRenderers;
    public CharacterObject OwnerObject { get; set; }

    protected virtual void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
    
    public abstract void Attack();  // 공격
    // public abstract void Aiming(bool isAiming, float aimTime);  // 조준
}
