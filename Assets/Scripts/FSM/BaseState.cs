using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 상태의 베이스(부모)가 되는 클래스
public abstract class BaseState
{
    public BaseState(MotionHandler motionHandler) { }    

    public abstract void OnStateEnter(MotionHandler motionHandler);    // 해당 상태에 진입했을 때 호출하는 메서드
    public abstract void OnStateUpdate(MotionHandler motionHandler);    // 해당 상태 진행중일 때 호출하는 메서드
    public abstract void OnStateExit(MotionHandler motionHandler);    // 해당 상태에 빠져나갔을 때 호출하는 메서드
}
