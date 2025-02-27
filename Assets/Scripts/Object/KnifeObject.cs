using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeObject : WeaponObject
{
    public Action HitEvent;

    public override void Init(Data data)
    {
        base.Init(data);

        HitEvent = null;
    }

    public override void Take()
    {
        WeaponHUD.HitCanvasGroup.alpha = 0f;   // 히트 판정 캔버스 그룹 투명값 초기화
    }
    
    public override void Attack()
    {
        FSM.ChangeState(new AttackState(MotionHandler));

        var knife = data as KnifeInfo;        

        // 공격 사운드
        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(knife.AttackSoundId, transform.position, Quaternion.identity);
    }
}
