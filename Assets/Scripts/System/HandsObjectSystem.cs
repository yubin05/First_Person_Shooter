using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class HandsObjectSystem : MonoBehaviour, PlayerInputAction.IBattleField_HandChangeActions
{
    public enum Types { Rifle, Pistol, Knife }
    public Types Type { get; private set; }

    [SerializeField] private HandsObject rifleHandObject;
    // public HandsObject RifleHandObject => rifleHandObject;

    [SerializeField] private HandsObject pistolHandObject;
    // public HandsObject PistolHandObject => pistolHandObject;

    [SerializeField] private HandsObject knifeHandObject;
    // public HandsObject KnifeHandObject => knifeHandObject;

    public IChangeWeapon IChangeWeapon { get; private set; }    // character object
    public HandsObject CurHandsObject { get; private set; }

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

    public void Init(IChangeWeapon changeWeapon)
    {
        IChangeWeapon = changeWeapon;
        CurHandsObject = null;
    }

    // 손 오브젝트 교체
    public HandsObject Change<K>()
    {
        switch (typeof(K).Name)
        {
            case nameof(RifleObject): default:
            {
                pistolHandObject.gameObject.SetActive(false);
                knifeHandObject.gameObject.SetActive(false);
                rifleHandObject.gameObject.SetActive(true);                

                Type = Types.Rifle;
                CurHandsObject = rifleHandObject;
                break;
            }
            case nameof(PistolObject):
            {
                rifleHandObject.gameObject.SetActive(false);
                knifeHandObject.gameObject.SetActive(false);
                pistolHandObject.gameObject.SetActive(true);

                Type = Types.Pistol;
                CurHandsObject = pistolHandObject;
                break;
            }
            case nameof(KnifeObject):
            {
                rifleHandObject.gameObject.SetActive(false);
                pistolHandObject.gameObject.SetActive(false);
                knifeHandObject.gameObject.SetActive(true);

                Type = Types.Knife;
                CurHandsObject = knifeHandObject;
                break;
            }
        }

        return CurHandsObject;
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
