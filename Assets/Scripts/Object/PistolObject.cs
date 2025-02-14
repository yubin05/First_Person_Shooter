using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolObject : GunObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        ShotMode = ShotModes.SemiAuto;
    }

    public new void ChangeShotMode()
    {
    }
}
