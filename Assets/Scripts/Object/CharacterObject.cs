using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterObject : EntityObject
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    public MotionHandler MotionHandler { get; protected set; }

    public FSM FSM { get; protected set; }

    // 추후에 위치 변경해야 함
    public GunObject GunObject { get; set; }    // 현재 플레이어가 장착하고 있는 총 오브젝트

    // 부위
    [Header("부위 노드")]
    [SerializeField] protected Transform gunNode;
    public Transform GunNode => gunNode;

    protected virtual void Awake()
    {
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init(this);

        FSM = new FSM(this);
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        FSM.ChangeState(new IdleState(this));
    }

    public virtual void OnMove(Vector3 moveVec)
    {
        FSM.ChangeState(new MoveState(this));
        transform.Translate(moveVec, Space.Self);
    }

    // 공격
    public virtual void OnAttack()
    {
        if (GunObject == null) return;
        FSM.ChangeState(new AttackState(this));

        GunObject.Shot();
    }

    // 조준
    public virtual void OnAiming(bool isAiming, float aimTime)
    {
        if (GunObject == null) return;
        FSM.ChangeState(new AimState(this));

        var gun = GunObject.data as GunInfo;
        GunNode.transform.DOLocalMove(gun.GetAimPos(isAiming), aimTime);
        GunObject.SetAlpha(isAiming ? 0f : 1f);
    }
}
