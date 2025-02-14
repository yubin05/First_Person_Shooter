using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : WeaponInfo
{
    public int BulletId { get; set; }
    public int ShotSoundId { get; set; }
    public float ShotSpeed { get; set; }
    public float ShotDelay => Mathf.Clamp(1f-ShotSpeed, 0.01f, 1f-0.01f);
    public float Distance { get; set; }
    public float MinVerReact { get; set; }
    public float MaxVerReact { get; set; }
    public float MinHorReact { get; set; }
    public float MaxHorReact { get; set; }    

    // 컨트롤러에서 직접 설정
    public float AimingTime { get; set; }
}
