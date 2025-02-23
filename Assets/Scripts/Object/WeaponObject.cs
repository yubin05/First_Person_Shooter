using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponObject : EntityObject
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    [Header("HUD")]
    [SerializeField] protected WeaponHUD weaponHUD;
    public WeaponHUD WeaponHUD => weaponHUD;

    public MotionHandler MotionHandler { get; protected set; }
    public FSM FSM { get; protected set; }    

    protected MeshRenderer[] meshRenderers;
    public CharacterObject OwnerObject { get; set; }

    protected virtual void Awake()
    {
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        WeaponHUD.Init();

        MotionHandler.Init();
        FSM = new FSM(MotionHandler);        

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
    
    public abstract void Take();    // 장착
    public abstract void Attack();  // 공격
}
