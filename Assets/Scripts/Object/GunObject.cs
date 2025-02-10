using System;
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
        
        GameApplication.Instance.GameController.BulletController.Spawn<Bullet, BulletObject>(gun.BulletId, this);   // 총알 소환
        ShotEvent?.Invoke();
    }
    // 조준
    public override void Aiming(Transform weaponNode, bool isAiming, float aimTime)
    {
        var gun = data as GunInfo;

        weaponNode.transform.DOLocalMove(gun.GetAimPos(isAiming), aimTime);
        AimCanvasGroup.DOFade(isAiming ? 0f : 1f, 0.1f);
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
