using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : EntityObject, IDamageable, IChangeWeapon
{
    // 카메라
    [SerializeField] protected Camera cam;
    public Camera Cam => cam;
    
    // 1인칭 손 오브젝트 관련
    [SerializeField] protected HandsObjectSystem handsObjectSystem;
    public HandsObjectSystem HandsObjectSystem => handsObjectSystem;

    // 애니메이터 & 부위 관련
    protected Animator animator;
    public Animator Animator
    {
        get => animator;
        protected set
        {
            animator = value;
            MotionHandler = animator.transform.GetComponent<MotionHandler>(); MotionHandler.Init();
            FSM = new FSM(MotionHandler);
        }
    }
    public MotionHandler MotionHandler { get; protected set; }
    public FSM FSM { get; protected set; }
    public Transform WeaponNode { get; protected set; }

    public WeaponObject WeaponObject { get; set; }    // 현재 플레이어가 장착하고 있는 무기

    public HealthSystem HealthSystem { get; set; }

    // 캐릭터 오브젝트가 움직이고 있는 지 체크
    // -> MotionHandler의 IsMove는 애니메이션의 IsMove
    // 해당 IsMove는 캐릭터 오브젝트의 IsMove
    public bool IsMove { get; protected set; }

    protected virtual void Awake()
    {
        HealthSystem = GetComponent<HealthSystem>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init(this);

        IsMove = false;
        
        HandsObjectSystem?.Init(this);
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        if (!MotionHandler.IsAttack && !MotionHandler.IsReload && !MotionHandler.IsAiming && !MotionHandler.IsTake) FSM.ChangeState(new IdleState(MotionHandler));
        IsMove = false;
    }

    public virtual void OnMove(Vector3 moveVec)
    {
        if (!MotionHandler.IsAttack && !MotionHandler.IsReload && !MotionHandler.IsAiming && !MotionHandler.IsTake) FSM.ChangeState(new MoveState(MotionHandler));
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
        HandsObjectSystem.CurHandsObject.Aiming(isAiming, character.BasicActorStat.AimingTime);
    }

    // 피격
    public virtual void OnHit(float attackPower)
    {
        FSM.ChangeState(new HitState(MotionHandler));
        HealthSystem.CurHp.Value -= attackPower;
    }
    
    // 죽음
    public virtual void OnDeath()
    {
        FSM.ChangeState(new DeathState(MotionHandler));
        
        var character = data as Character;
        character.RemoveData(); // 죽으면 데이터 삭제
    }

    // 무기 장착(교체)
    public virtual K OnTake<T, K>(int weaponId) where T : WeaponInfo where K : WeaponObject
    {
        // 조준 중이면 조준 해제
        if (HandsObjectSystem.CurHandsObject != null)
        {
            if (HandsObjectSystem.CurHandsObject.WeaponObject)
            {
                if (HandsObjectSystem.CurHandsObject.WeaponObject is GunObject)
                {
                    // 조준 해제
                    var gunObject = HandsObjectSystem.CurHandsObject.WeaponObject as GunObject;
                    gunObject.Aiming(false);

                    // 기존 총 장전 해제 및 사운드 데이터 제거
                    gunObject.MotionHandler.EndReload();
                    if (gunObject.ReloadSoundObj != null) gunObject.ReloadSoundObj.data.RemoveData();
                }
            }
        }

        HandsObjectSystem.Change<K>();    // 손 오브젝트 교체

        // 애니메이터 및 부위 노드 설정
        Animator = HandsObjectSystem.CurHandsObject.Animator;
        WeaponNode = HandsObjectSystem.CurHandsObject.WeaponNode;

        // 무기 소환
        if (HandsObjectSystem.CurHandsObject.WeaponObject == null)
        {
            switch (typeof(T).Name)
            {
                case nameof(GunInfo): default: HandsObjectSystem.CurHandsObject.WeaponObject = GameApplication.Instance.GameController.GunController.Spawn<T, K>(weaponId, this); break;
                case nameof(KnifeInfo): HandsObjectSystem.CurHandsObject.WeaponObject = GameApplication.Instance.GameController.KnifeController.Spawn<T, K>(weaponId, this); break;
            }
        }
        // 애니메이터 설정 시 모션 핸들러 이벤트가 모두 초기화되므로 나이프의 경우, 공격 이벤트 추가해줘야 함
        if (HandsObjectSystem.CurHandsObject.WeaponObject is KnifeObject)
        {
            var knifeObj = HandsObjectSystem.CurHandsObject.WeaponObject as KnifeObject;
            var knife = knifeObj.data as KnifeInfo;

            knifeObj.OwnerObject.MotionHandler.AttackEvent += () => 
            {
                var hits = Physics.OverlapSphere(knifeObj.OwnerObject.Cam.transform.position, knife.Distance, LayerMask.GetMask(nameof(Enemy)));
                if (hits.Length > 0)
                {
                    var hitInstances = new HashSet<int>();  // 콜라이더 탐지에 있어서 객체 중복을 막기 위해 추가
                    foreach (var hit in hits)
                    {
                        var damageSystem = hit.transform.GetComponentInParent<DamageSystem>();
                        if (damageSystem != null)
                        {
                            int instanceId = damageSystem.GetInstanceID();

                            // 한번도 탐지하지 않은 객체 아이디만 히트 판정 처리
                            if (!hitInstances.Contains(instanceId))
                            {
                                hitInstances.Add(instanceId);

                                damageSystem.OnHit(knife.AttackPower);
                                knifeObj.HitEvent?.Invoke();
                            }                    
                        }
                    }
                }
            };
        }        
        WeaponObject = HandsObjectSystem.CurHandsObject.WeaponObject;
        WeaponObject.Take(); WeaponObject.gameObject.SetActive(true);
        
        // 무기 장착 상태로 변경
        FSM.ChangeState(new TakeState(MotionHandler));

        return WeaponObject as K;
    }
}
