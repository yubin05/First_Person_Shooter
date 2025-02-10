using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(MotionHandler motionHandler) : base(motionHandler) {}

    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.StartMove();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
    }
}
