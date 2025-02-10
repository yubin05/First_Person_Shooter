using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunInputSystem : MonoBehaviour, PlayerInputAction.IBattleField_GunActions
{
    [SerializeField] private GunObject gunObject;

    private PlayerInputAction inputAction;
    private float shotDelayTime;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        shotDelayTime = 0f;
    }

    private void OnEnable()
    {
        inputAction.BattleField_Gun.SetCallbacks(this);
        inputAction.BattleField_Gun.Enable();
    }

    private void Update()
    {
        if (BattleFieldScene.Instance.IsPause) return;

        var gun = gunObject.data as GunInfo;

        // 공격
        if (gunObject.ShotMode == GunObject.ShotModes.Auto)
        {
            var shotSpeed = Mathf.Clamp(1f-gun.ShotSpeed, 0.01f, 1f-0.01f);
            if (shotDelayTime > 0.1f)
            {
                var inputAttackValue = inputAction.BattleField_Gun.Shot.ReadValue<float>();
                if (inputAttackValue == 1f)
                {
                    gunObject.OwnerObject.OnAttack();
                    shotDelayTime = 0f;
                }
            }
        }
        shotDelayTime += Time.deltaTime;

        // 조준
        var inputAimingValue = inputAction.BattleField_Gun.Aiming.ReadValue<float>();
        if (inputAimingValue == 1f) gunObject.OwnerObject.OnAiming(true, gun.AimingTime);
        else gunObject.OwnerObject.OnAiming(false, gun.AimingTime);
    }

    private void OnDisable()
    {
        inputAction.BattleField_Gun.Disable();
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (gunObject.ShotMode == GunObject.ShotModes.SemiAuto)
        {
            var inputValue = context.ReadValue<float>();
            if (inputValue == 1f) gunObject.OwnerObject.OnAttack();
        }
    }
    public void OnAiming(InputAction.CallbackContext context)
    {
    }
    public void OnShotMode(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;

        var inputValue = context.ReadValue<float>();
        if (inputValue == 0)
        {
            int index = (int)gunObject.ShotMode;
            if (++index >= Enum.GetValues(typeof(GunObject.ShotModes)).Length) index = 0;
            gunObject.ShotMode = (GunObject.ShotModes)index;
        }
    }
}
