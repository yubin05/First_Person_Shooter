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
    
    [Header("캔버스 그룹")]
    [SerializeField] protected CanvasGroup aimCanvasGroup;
    public CanvasGroup AimCanvasGroup => aimCanvasGroup;
    [SerializeField] protected CanvasGroup hitCanvasGroup;
    public CanvasGroup HitCanvasGroup => hitCanvasGroup;

    public enum ShotModes { SemiAuto, Auto } // 단발, 연사
    public ShotModes ShotMode { get; set; }

    protected Coroutine blinkCoroutine;
    public event Action ShotEvent;

    public override void Init(Data data)
    {
        base.Init(data);

        blinkCoroutine = null;
        ShotEvent = null;
    }

    // 총알 발사
    public override void Attack()
    {
        var gun = data as GunInfo;

        // 레이캐스트로 대상 체크
        bool isHit = Physics.Raycast(OwnerObject.Cam.transform.position, OwnerObject.Cam.transform.forward, out RaycastHit hit, gun.Distance, LayerMask.GetMask(nameof(Player), nameof(Obstacle)));
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

    // 발사 모드 변경
    public virtual void ChangeShotMode()
    {
        int index = (int)ShotMode;
        if (++index >= Enum.GetValues(typeof(ShotModes)).Length) index = 0;
        ShotMode = (ShotModes)index;
    }

    public void BlinkHitCanvas(float delayTime)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkProcess(delayTime));
    }
    protected IEnumerator BlinkProcess(float delayTime)
    {
        HitCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(delayTime);
        HitCanvasGroup.alpha = 0f;
    }
}
