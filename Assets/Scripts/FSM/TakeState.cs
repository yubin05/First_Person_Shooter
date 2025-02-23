using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeState : BaseState
{
    public TakeState(MotionHandler motionHandler) : base(motionHandler) {}
    
    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.StartTake();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
    }
}
