using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KnifeInputSystem : WeaponInputSystem, PlayerInputAction.IBattleField_KnifeActions
{
    [SerializeField] private KnifeObject knifeObject;

    private PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void Start()
    {
        inputAction.BattleField_Knife.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputAction.BattleField_Knife.Enable();
    }

    private void OnDisable()
    {
        inputAction.BattleField_Knife.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (BattleFieldScene.Instance.IsPause) return;
        if (knifeObject.OwnerObject.MotionHandler.IsAttack) return;

        var value = context.ReadValueAsButton();
        if (value == false)
        {
            knifeObject.OwnerObject.OnAttack();
        }
    }
}
