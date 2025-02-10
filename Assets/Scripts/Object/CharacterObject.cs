using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterObject : EntityObject, IDamageable
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    public MotionHandler MotionHandler { get; protected set; }
    public FSM FSM { get; protected set; }

    public WeaponObject WeaponObject { get; set; }    // 현재 플레이어가 장착하고 있는 무기

    public HealthSystem HealthSystem { get; set; }

    // 부위
    [Header("부위 노드")]
    [SerializeField] protected Transform weaponNode;
    public Transform WeaponNode => weaponNode;

    protected virtual void Awake()
    {
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
        HealthSystem = GetComponent<HealthSystem>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init(this);

        FSM = new FSM(MotionHandler);
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        FSM.ChangeState(new IdleState(MotionHandler));
    }

    public virtual void OnMove(Vector3 moveVec)
    {
        FSM.ChangeState(new MoveState(MotionHandler));
        transform.Translate(moveVec, Space.Self);
    }

    // 공격
    public virtual void OnAttack()
    {
        if (WeaponObject == null) return;
        FSM.ChangeState(new AttackState(MotionHandler));

        WeaponObject.Attack();
    }

    // 조준
    public virtual void OnAiming(bool isAiming, float aimTime)
    {
        if (WeaponObject == null) return;
        FSM.ChangeState(new AimState(MotionHandler));

        WeaponObject.Aiming(WeaponNode, isAiming, aimTime);
    }

    // 피격
    public virtual void OnHit(float attackPower)
    {
        FSM.ChangeState(new HitState(MotionHandler));
        HealthSystem.CurHp -= attackPower;
    }
    
    public virtual void OnDeath()
    {
        FSM.ChangeState(new DeathState(MotionHandler));
        
        var character = data as Character;
        character.RemoveData(); // 죽으면 데이터 삭제
    }
}
