using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunObject : WeaponObject
{
    [Header("부위 노드")]
    [SerializeField] protected Transform muzzleNode;    // 총구
    public Transform MuzzleNode => muzzleNode;

    public enum ShotModes { SemiAuto, Auto } // 단발, 연사
    public ShotModes ShotMode { get; set; }

    public event Action ShotEvent;

    public override void Init(Data data)
    {
        base.Init(data);

        var gun = data as GunInfo;

        MotionHandler.ReloadEvent += () => 
        {
            gun.Reload();
            WeaponHUD.UpdateAmmoTxt(gun.CurMagazineCapacity, gun.ReserveAmmo);
        };
        MotionHandler.OnReload();

        ShotEvent = null;
        ShotEvent += () => 
        {
            gun.Shot();
            WeaponHUD.UpdateAmmoTxt(gun.CurMagazineCapacity, gun.ReserveAmmo);
        };
    }

    // 총알 발사
    public override void Attack()
    {
        FSM.ChangeState(new AttackState(MotionHandler));

        var gun = data as GunInfo;

        var direction = OwnerObject.Cam.transform.forward;
        // 플레이어가 움직이는 중이거나 반동 중일 때는 탄 퍼짐
        if (OwnerObject.IsMove || (OwnerObject.Cam.TryGetComponent<ReactionSystem>(out var reactionSystem) && reactionSystem.IsReact))
        {
            direction.x += UnityEngine.Random.Range(gun.MinHorBulletSpread, gun.MaxHorBulletSpread);
            direction.y += UnityEngine.Random.Range(gun.MinVerBulletSpread, gun.MaxVerBulletSpread);
        }

        // 레이캐스트로 대상 체크        
        bool isHit = Physics.Raycast(OwnerObject.Cam.transform.position, direction, out RaycastHit hit, gun.Distance, LayerMask.GetMask(nameof(Player), nameof(Obstacle)));
        Vector3 targetPos = isHit ? hit.point : OwnerObject.Cam.transform.position + OwnerObject.Cam.transform.forward*gun.Distance;        
        
        var bulletObj = GameApplication.Instance.GameController.BulletController.Spawn<Bullet, BulletObject>(gun.BulletId, this, targetPos);   // 총알 소환
        var bullet = bulletObj.data as Bullet;

        // 히트 스캔의 경우, 건 오브젝트에서 즉시 대미지 처리해도 차이 없음
        if (bullet.Type == Bullet.Types.HitScan)
        {
            if (isHit)
            {
                var damageSystem = hit.transform.GetComponentInParent<DamageSystem>();
                if (damageSystem != null)
                {
                    damageSystem.OnHit(bullet.AttackPower);
                    bulletObj.ExecuteHitEvent();
                }
            }
        }

        ShotEvent?.Invoke();
    }
    // 조준
    // public override void Aiming(bool isAiming, float aimTime)
    // {
    //     var gun = data as GunInfo;

    //     AimCanvasGroup.DOFade(isAiming ? 0f : 1f, 0.1f);
    // }

    // 재장전
    public virtual void Reload()
    {
        var gun = data as GunInfo;
        if (gun.CurMagazineCapacity >= gun.MagazineCapacity || gun.ReserveAmmo <= 0) return;

        OwnerObject.FSM.ChangeState(new ReloadState(OwnerObject.MotionHandler));
        FSM.ChangeState(new ReloadState(MotionHandler));
        
        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(gun.ReloadSoundId, Vector3.zero, Quaternion.identity, transform);
    }

    // 발사 모드 변경
    public virtual void ChangeShotMode()
    {
        int index = (int)ShotMode;
        if (++index >= Enum.GetValues(typeof(ShotModes)).Length) index = 0;
        ShotMode = (ShotModes)index;
    }
}
