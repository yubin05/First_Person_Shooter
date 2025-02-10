using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponObject : EntityObject
{
    public CharacterObject OwnerObject { get; set; }
    public abstract void Attack();  // 공격
    public abstract void Aiming(Transform weaponNode, bool isAiming, float aimTime);  // 조준
}
