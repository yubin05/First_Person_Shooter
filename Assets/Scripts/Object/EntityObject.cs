using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityObject : PoolObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        var entity = data as Entity;
        entity.Init(this);
    }
}
