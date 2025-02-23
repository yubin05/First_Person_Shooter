using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HandsData", menuName = "Scriptable Object/HandsData", order = int.MaxValue-1)]
public class HandsData : ScriptableObject
{
    [SerializeField] private int weaponId;
    public int WeaponId => weaponId;
    
    [SerializeField] private Vector3 aimLocalVec;
    public Vector3 AimLocalVec => aimLocalVec;

    public Vector3 NoAimLocalVec { get; set; }
}
