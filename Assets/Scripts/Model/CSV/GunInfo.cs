using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : WeaponInfo
{
    public int BulletId { get; set; }
    public int ShotSoundId { get; set; }
    public float ShotSpeed { get; set; }    
    public float Distance { get; set; }
    public int ReloadSoundId { get; set; }
    public int MagazineCapacity { get; set; }
    public int TotalAmmo { get; set; }
    public float MinVerBulletSpread { get; set; }
    public float MaxVerBulletSpread { get; set; }
    public float MinHorBulletSpread { get; set; }
    public float MaxHorBulletSpread { get; set; }
    public int HasAimingMode { get; set; }
    public float AimRate { get; set; }

    // 외부에서 Set하는 변수들
    public float AimingTime { get; set; }
    private float MinShotSpeed { get; } = 0.01f; private float MaxShotSpeed { get; } = 1f;
    public float ShotDelay => Mathf.Clamp(MaxShotSpeed-ShotSpeed, MinShotSpeed, MaxShotSpeed-MinShotSpeed);
    public int CurMagazineCapacity { get; private set; }    // 현재 탄창의 남아있는 총알 수
    public int ReserveAmmo { get; private set; }    // 전체적으로 남아있는 총알 수    
    public float NoZoomFieldOfView { get; set; }    // 조준하지 않았을 때, 캐릭터의 카메라 시야각
    public float ZoomFieldOfView { get; set; }    // 조준했을 때, 캐릭터의 카메라 시야각

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        CurMagazineCapacity = MagazineCapacity;
        ReserveAmmo = TotalAmmo-MagazineCapacity;
    }

    public void Shot()
    {
        if (CurMagazineCapacity <= 0) return;
        CurMagazineCapacity -= 1;
    }
    public void Reload()
    {
        var chargeAmmo = MagazineCapacity-CurMagazineCapacity;
        chargeAmmo = Mathf.Min(chargeAmmo, ReserveAmmo);
        CurMagazineCapacity += chargeAmmo; ReserveAmmo -= chargeAmmo;
    }
}
