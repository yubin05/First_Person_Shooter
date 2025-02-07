using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunObject : EntityObject
{
    [Header("부위 노드")]
    [SerializeField] protected Transform muzzleNode;    // 총구
    public Transform MuzzleNode => muzzleNode;
    
    [Header("캔버스 그룹")]
    [SerializeField] protected CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup => canvasGroup;

    // 총알 발사
    public void Shot()
    {
        var gun = data as GunInfo;
        GameApplication.Instance.GameController.BulletController.Spawn<Bullet, BulletObject>(gun.BulletId, this);
    }

    public void SetAlpha(float alpha, float tweenTime=0.1f)
    {
        if (canvasGroup == null) return;
        canvasGroup.DOFade(alpha, tweenTime);
    }    
}
