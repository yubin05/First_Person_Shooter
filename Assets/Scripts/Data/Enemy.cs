using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float RespawnTime { get; set; }
    
    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);
    }
}
