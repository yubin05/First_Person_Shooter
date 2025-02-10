using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : EntityObject, ICollider
{
    public event Action HitEvent;

    public override void Init(Data data)
    {
        base.Init(data);
        HitEvent = null;
    }

    private void Update()
    {
        var bullet = data as Bullet;
        transform.Translate(Vector3.forward*bullet.MoveSpeed, Space.Self);
    }

    public void OnTriggerEnter(Collider other)
    {
        var damageSystem = other.GetComponentInParent<DamageSystem>();
        if (damageSystem != null)
        {
            var bullet = data as Bullet;
            
            damageSystem.OnHit(bullet.AttackPower);
            HitEvent?.Invoke();
            
            bullet.RemoveData();    // 한번 히트하면 사라져야 함 (관통탄은 추후에 예외 처리)
        }
    }

    public void OnTriggerExit(Collider other)
    {
    }
}
