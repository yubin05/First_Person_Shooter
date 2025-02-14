using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(MotionHandler motionHandler) : base(motionHandler) {}
    
    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.StartAttack();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
    }
}
