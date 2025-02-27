using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    public enum Types { HitScan }
    public Types Type { get; set; }

    public float AttackPower { get; set; }
    public float MoveSpeed { get; set; }

    // 컨트롤러에서 설정
    public Vector3 TargetPos { get; set; }
}
