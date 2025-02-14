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
    public float MinVerReact { get; set; }
    public float MaxVerReact { get; set; }
    public float MinHorReact { get; set; }
    public float MaxHorReact { get; set; }    

    // 외부에서 Set하는 변수들
    public float AimingTime { get; set; }
    public float ShotDelay => Mathf.Clamp(1f-ShotSpeed, 0.01f, 1f-0.01f);
    public int CurMagazineCapacity { get; private set; }    // 현재 탄창의 남아있는 총알 수
    public int ReserveAmmo { get; private set; }    // 전체적으로 남아있는 총알 수

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
