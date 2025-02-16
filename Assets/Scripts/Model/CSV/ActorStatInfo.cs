using System;
using UnityEngine;

public class ActorStatInfo : Data
{
    public float MaxHp { get; set; }
    public float MoveSpeed { get; set; }
    public float AimingSpeed { get; set; }

    // Other
    private float MinAimingSpeed { get; } = 0.01f; private float MaxAimingSpeed { get; } = 1f;
    public float AimingTime => Mathf.Clamp(MaxAimingSpeed-AimingSpeed, MinAimingSpeed, MaxAimingSpeed-MinAimingSpeed);
}
