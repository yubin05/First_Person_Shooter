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

    // 무기 입력 시스템 - 플레이어일 경우에는 키고 적일 경우에는 끄기 위함
    [Header("Weapon Input System")]
    [SerializeField] protected WeaponInputSystem weaponInputSystem;
    public WeaponInputSystem WeaponInputSystem => weaponInputSystem;

    public MotionHandler MotionHandler { get; protected set; }
    public FSM FSM { get; protected set; }    

    public CharacterObject OwnerObject { get; set; }    

    protected virtual void Awake()
    {
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        WeaponHUD.Init();

        MotionHandler.Init();
        FSM = new FSM(MotionHandler);
    }
    
    public abstract void Take();    // 장착
    public abstract void Attack();  // 공격
}
