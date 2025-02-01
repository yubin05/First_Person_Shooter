using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(CharacterObject characterObject) : base(characterObject) { }

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.MotionHandler.EndMove();
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
    }
}
