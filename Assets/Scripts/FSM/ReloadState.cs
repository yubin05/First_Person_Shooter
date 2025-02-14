using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : BaseState
{
    public ReloadState(MotionHandler motionHandler) : base(motionHandler) {}

    public override void OnStateEnter(MotionHandler motionHandler)
    {
        motionHandler.StartReload();
    }

    public override void OnStateUpdate(MotionHandler motionHandler)
    {
    }

    public override void OnStateExit(MotionHandler motionHandler)
    {
    }
}
