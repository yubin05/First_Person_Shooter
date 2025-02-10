using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : WeaponInfo
{
    public int BulletId { get; set; }

    public int ShotSoundId { get; set; }
    public float ShotSpeed { get; set; }

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

    public float MinVerReact { get; set; }
    public float MaxVerReact { get; set; }
    public float MinHorReact { get; set; }
    public float MaxHorReact { get; set; }

    // 컨트롤러에서 직접 설정
    public float AimingTime { get; set; }
}
