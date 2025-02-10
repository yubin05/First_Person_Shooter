using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleObject : GunObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        ShotMode = ShotModes.Auto;
    }
}
