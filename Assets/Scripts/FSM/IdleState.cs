using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(MotionHandler motionHandler) : base(motionHandler) { }

    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.EndMove();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
    }
}
