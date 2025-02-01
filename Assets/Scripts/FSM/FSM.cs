using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유한 상태 머신 - 상태를 한 가지로 유한하게 한정해주는 클래스
// Ex) 캐릭터의 상태(Idle, Move, Attack 등)을 한 가지로 한정해주고 할 때 사용할 수 있음
public class FSM
{
    private CharacterObject characterObject;    // 현재 상태 머신이 제어하고 있는 캐릭터 오브젝트
    public BaseState CurState { get; private set; } // 현재 상태를 담고 있는 변수 - 모든 상태 클래스는 BaseState를 상속받아야 함

    public FSM(CharacterObject characterObject)
    {
        this.characterObject = characterObject;
        ChangeState(new IdleState(characterObject));
    }

    public void ChangeState(BaseState nextState)
    {
        if (CurState != null)
            CurState.OnStateExit(characterObject);   // 현재 상태가 존재하면 상태를 빠져나갈 때 사용하는 함수 호출

        // 다음 상태를 현재 상태로 바꾸고 해당 상태를 진입하는 함수 호출
        CurState = nextState;
        CurState.OnStateEnter(characterObject);
    }

    // 현재 상태를 계속 호출하는 함수
    public void UpdateState()
    {
        CurState.OnStateUpdate(characterObject);
    }
}
