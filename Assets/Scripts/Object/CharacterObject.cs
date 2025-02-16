using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterObject : EntityObject, IDamageable
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;
    
    [SerializeField] protected Camera cam;
    public Camera Cam => cam;

    [SerializeField] protected HandsObject handsObject;
    public HandsObject HandsObject => handsObject;

    public MotionHandler MotionHandler { get; protected set; }
    public FSM FSM { get; protected set; }    

    public WeaponObject WeaponObject { get; set; }    // 현재 플레이어가 장착하고 있는 무기

    public HealthSystem HealthSystem { get; set; }

    // 캐릭터 오브젝트가 움직이고 있는 지 체크
    // -> MotionHandler의 IsMove는 애니메이션이 IsMove인지 체크하는 것이기에 다름
    public bool IsMove { get; protected set; }

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

        MotionHandler.Init();
        FSM = new FSM(MotionHandler);

        IsMove = false;
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        if (!MotionHandler.IsAttack && !MotionHandler.IsReload && !MotionHandler.IsAiming) FSM.ChangeState(new IdleState(MotionHandler));
        IsMove = false;
    }

    public virtual void OnMove(Vector3 moveVec)
    {
        if (!MotionHandler.IsAttack && !MotionHandler.IsReload && !MotionHandler.IsAiming) FSM.ChangeState(new MoveState(MotionHandler));
        transform.Translate(moveVec, Space.Self); IsMove = true;
    }

    // 공격
    public virtual void OnAttack()
    {
        if (WeaponObject is GunObject)
        {
            var gunObject = WeaponObject as GunObject;
            if (gunObject.MotionHandler.IsReload) return;

            var gun = gunObject.data as GunInfo;
            if (gun.CurMagazineCapacity <= 0) return;
        }

        FSM.ChangeState(new AttackState(MotionHandler));

        WeaponObject.Attack();
    }

    // 조준
    public virtual void OnAiming(bool isAiming)
    {
        if (isAiming) FSM.ChangeState(new AimState(MotionHandler));

        var character = data as Character;
        HandsObject.Aiming(isAiming, character.BasicActorStat.AimingTime);
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
