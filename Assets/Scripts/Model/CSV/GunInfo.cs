using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : Entity
{
    public int BulletId { get; set; }
    public int SoundId { get; set; }

    public float NoAimPosX { get; set; }
    public float NoAimPosY { get; set; }
    public float NoAimPosZ { get; set; }
    public float AimPosX { get; set; }
    public float AimPosY { get; set; }
    public float AimPosZ { get; set; }

    public Vector3 GetAimPos(bool isAiming)
    {
        if (isAiming) return new Vector3(AimPosX, AimPosY, AimPosZ);
        else return new Vector3(NoAimPosX, NoAimPosY, NoAimPosZ);
    } 
}
