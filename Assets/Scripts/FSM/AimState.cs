using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : BaseState
{
    public AimState(MotionHandler motionHandler) : base(motionHandler) {}

    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.StartAiming();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
        motionHandler.EndAiming();
    }
}
