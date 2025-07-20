using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyObject : CharacterObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        var enemy = data as Enemy;
        enemy.Init(this);
    }

    // temp
    private void Update()
    {
        var enemy = data as Enemy;

        var nearestObj = GetNearestTrans();
        if (nearestObj != null)
        {
            OnTargeting(nearestObj);

            // 공격 딜레이
            if (enemy.AttackDelayTime > enemy.AttackDelay)
            {
                enemy.AttackDelayTime = 0f;
                OnAttack();
            }
        }
        enemy.AttackDelayTime += Time.deltaTime;
    }

    // 타깃 플레이어 체크
    public virtual void OnTargeting(Transform _transform)
    {
        transform.LookAt(_transform);
    }

    // 타깃 범위 안에 있는 플레이어 트랜스폼 체크
    public virtual Transform GetNearestTrans()
    {
        var players = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        if (players == null) return null;

        var enemy = data as Enemy;
        var nearestPlayerObj = players
            .Where(player => Vector3.Magnitude(player.MyObject.transform.position - transform.position) < enemy.DetectDistance)
            .OrderBy(player => Vector3.Distance(player.MyObject.transform.position, transform.position))
            .FirstOrDefault();

        if (nearestPlayerObj == null) return null;
        return nearestPlayerObj.MyObject.transform;
    }
}