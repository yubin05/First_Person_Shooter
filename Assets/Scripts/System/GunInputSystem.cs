using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
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
            if (shotDelayTime > gun.ShotDelay)
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
        if (!gunObject.MotionHandler.IsReload)
        {
            var inputAimingValue = inputAction.BattleField_Gun.Aiming.ReadValue<float>();
            if (inputAimingValue == 1f)
            {
                gunObject.Aiming(true);
            }
            else
            {
                gunObject.Aiming(false);
            }            
        }
        else
        {
            gunObject.Aiming(false);
        }
    }

    private void OnDisable()
    {
        inputAction.BattleField_Gun.Disable();
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        
        if (gunObject.ShotMode == GunObject.ShotModes.SemiAuto)
        {
            var gun = gunObject.data as GunInfo;
            if (shotDelayTime > gun.ShotDelay)
            {
                var inputValue = context.ReadValue<float>();
                if (inputValue == 1f)
                {
                    gunObject.OwnerObject.OnAttack();
                    shotDelayTime = 0f;
                }
            }            
        }
    }
    public void OnAiming(InputAction.CallbackContext context)
    {
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        if (gunObject.MotionHandler.IsReload) return;

        var inputValue = context.ReadValue<float>();
        if (inputValue == 0)
        {
            gunObject.Reload();
        }
    }
}
