using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerHandsObjectSystem : HandsObjectSystem, PlayerInputAction.IBattleField_HandChangeActions
{
    private PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.BattleField_HandChange.SetCallbacks(this);
        inputAction.BattleField_HandChange.Enable();
    }

    private void OnDisable()
    {
        inputAction.BattleField_HandChange.Disable();
    }

    public void OnRifleChange(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        if (Type == Types.Rifle) return;

        var value = context.ReadValueAsButton();
        if (value == false) IChangeWeapon.OnTake<GunInfo, RifleObject>(90001);
    }
    public void OnPistolChange(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        if (Type == Types.Pistol) return;

        var value = context.ReadValueAsButton();
        if (value == false) IChangeWeapon.OnTake<GunInfo, PistolObject>(90002);
    }
    public void OnKnifeChange(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        if (Type == Types.Knife) return;

        var value = context.ReadValueAsButton();
        if (value == false) IChangeWeapon.OnTake<KnifeInfo, KnifeObject>(120001);
    }
}
