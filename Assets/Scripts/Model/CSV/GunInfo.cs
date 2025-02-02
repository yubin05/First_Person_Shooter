using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : Entity
{
    public float NoAimPosX { get; set; }
    public float NoAimPosY { get; set; }
    public float NoAimPosZ { get; set; }
    public float NoAimRotX { get; set; }
    public float NoAimRotY { get; set; }
    public float NoAimRotZ { get; set; }
    public float NoAimScale { get; set; }
    public float AimPosX { get; set; }
    public float AimPosY { get; set; }
    public float AimPosZ { get; set; }
    public float AimRotX { get; set; }
    public float AimRotY { get; set; }
    public float AimRotZ { get; set; }
    public float AimScale { get; set; }

    public Vector3 GetNoAimPos() => new Vector3(NoAimPosX, NoAimPosY, NoAimPosZ);
    public Vector3 GetAimPos() => new Vector3(AimPosX, AimPosY, AimPosZ);
    public Quaternion GetNoAimRot() => Quaternion.Euler(new Vector3(NoAimRotX, NoAimRotY, NoAimRotZ));
    public Quaternion GetAimRot() => Quaternion.Euler(new Vector3(AimRotX, AimRotY, AimRotZ));
}
