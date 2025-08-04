using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyObject))]
public class EnemyObject_AI : MonoBehaviour
{
    private EnemyObject enemyObject;

    public void Init(EnemyObject enemyObject)
    {
        this.enemyObject = enemyObject;
    }

    private void Update()
    {
        if (enemyObject == null) return;
        var enemy = enemyObject.data as Enemy;
        
        // 총을 장착하고 있으면서 총알이 없는 경우, 재장전 중이 아닌 경우에만 재장전
        if (enemyObject.WeaponObject.data is GunInfo gunInfo && gunInfo.CurMagazineCapacity <= 0
        && !enemyObject.MotionHandler.IsReload && !enemyObject.WeaponObject.MotionHandler.IsReload)
        {
            var gunObject = enemyObject.WeaponObject as GunObject;
            gunObject.Reload();
        }

        var nearestObj = GetNearestTrans();
        if (nearestObj != null)
        {
            OnTargeting(nearestObj);

            // 공격 딜레이
            if (enemy.AttackDelayTime > enemy.AttackDelay)
            {
                enemy.AttackDelayTime = 0f;
                enemyObject.OnAttack();
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
        // Debug.Log($"players.Count:{players.Count()}");
        if (players == null) return null;

        var enemy = enemyObject.data as Enemy;
        var nearestPlayerObj = players
            .Where(player => Vector3.Magnitude(player.MyObject.transform.position - transform.position) < enemy.DetectDistance)
            .OrderBy(player => Vector3.Distance(player.MyObject.transform.position, transform.position))
            .FirstOrDefault();

        if (nearestPlayerObj == null) return null;
        return nearestPlayerObj.MyObject.transform;
    }
}
