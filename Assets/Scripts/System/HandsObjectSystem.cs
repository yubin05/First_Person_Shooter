using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsObjectSystem : MonoBehaviour
{
    public enum Types { Rifle, Pistol, Knife }
    public Types Type { get; protected set; }

    [SerializeField] protected HandsObject rifleHandObject;
    // public HandsObject RifleHandObject => rifleHandObject;

    [SerializeField] protected HandsObject pistolHandObject;
    // public HandsObject PistolHandObject => pistolHandObject;

    [SerializeField] protected HandsObject knifeHandObject;
    // public HandsObject KnifeHandObject => knifeHandObject;

    public IChangeWeapon IChangeWeapon { get; protected set; }    // character object
    public HandsObject CurHandsObject { get; protected set; }

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
                pistolHandObject?.gameObject.SetActive(false);
                knifeHandObject?.gameObject.SetActive(false);
                rifleHandObject?.gameObject.SetActive(true);                

                Type = Types.Rifle;
                CurHandsObject = rifleHandObject;
                break;
            }
            case nameof(PistolObject):
            {
                rifleHandObject?.gameObject.SetActive(false);
                knifeHandObject?.gameObject.SetActive(false);
                pistolHandObject?.gameObject.SetActive(true);

                Type = Types.Pistol;
                CurHandsObject = pistolHandObject;
                break;
            }
            case nameof(KnifeObject):
            {
                rifleHandObject?.gameObject.SetActive(false);
                pistolHandObject?.gameObject.SetActive(false);
                knifeHandObject?.gameObject.SetActive(true);

                Type = Types.Knife;
                CurHandsObject = knifeHandObject;
                break;
            }
        }

        return CurHandsObject;
    }
}
