using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // CSV에서 파싱되는 데이터
    public float RespawnTime { get; set; }
    public float DetectDistance { get; set; }
    public float DetectAngle { get; set; }
    public float AttackDelay { get; set; }

    // CSV에서 파싱되지 않는 데이터
    public float AttackDelayTime;
    
    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);
        AttackDelayTime = 0f;
    }
}
