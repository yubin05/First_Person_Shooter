using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : EntityObject
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    public MotionHandler MotionHandler { get; protected set; }

    public FSM FSM { get; protected set; }

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
}
