using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : EntityObject
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

        // 히트 스캔의 경우, 오브젝트가 타겟 위치 근처에 도착 시, 삭제
        if (bullet.Type == Bullet.Types.HitScan)
        {
            if (Vector3.Distance(transform.position, bullet.TargetPos) <= 2f)
            {
                bullet.RemoveData();
            }
        }
    }

    public void ExecuteHitEvent() => HitEvent?.Invoke();
}
