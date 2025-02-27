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

    public SoundObject ReloadSoundObj { get; protected set; }

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

        ReloadSoundObj = null;
    }

    // 무기 장착
    public override void Take()
    {
        WeaponHUD.HitCanvasGroup.alpha = 0f;   // 히트 판정 캔버스 그룹 투명값 초기화
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
        bool isHit = Physics.Raycast(OwnerObject.Cam.transform.position, direction, out RaycastHit hit, gun.Distance, LayerMask.GetMask(nameof(Enemy), nameof(Obstacle)));
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
                    bulletObj.HitEvent?.Invoke();
                }
            }
        }

        ShotEvent?.Invoke();
    }

    // 조준
    public void Aiming(bool isAiming)
    {
        var gun = data as GunInfo;
        if (gun.HasAimingMode == 0) return;     // 조준 모드 없는 경우, 조준 하면 안됨

        // 캐릭터 오브젝트 조준 모드 - 손 오브젝트 위치 조정 포함
        OwnerObject.OnAiming(isAiming);
        
        var character = OwnerObject.data as Character;
        // 카메라 줌
        if (isAiming) OwnerObject.Cam.DOFieldOfView(gun.ZoomFieldOfView, character.BasicActorStat.AimingTime);
        else OwnerObject.Cam.DOFieldOfView(gun.NoZoomFieldOfView, character.BasicActorStat.AimingTime);

        // 조준선 UI FadeIn/FadeOut
        WeaponHUD.AimCanvasGroup.DOFade(isAiming ? 0f : 1f, 0.1f);
    }

    // 재장전
    public void Reload()
    {
        var gun = data as GunInfo;
        if (gun.CurMagazineCapacity >= gun.MagazineCapacity || gun.ReserveAmmo <= 0) return;

        OwnerObject.FSM.ChangeState(new ReloadState(OwnerObject.MotionHandler));
        FSM.ChangeState(new ReloadState(MotionHandler));
        
        ReloadSoundObj = GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(gun.ReloadSoundId, Vector3.zero, Quaternion.identity, transform);
        ReloadSoundObj.data.OnDataRemove += (data) =>
        {
            ReloadSoundObj = null;
        };
    }
}
