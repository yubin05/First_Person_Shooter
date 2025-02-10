using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public ActorStatInfo BasicActorStat { get; set; } // 기본 스탯    
    public Skills Skills { get; set; }  // 스킬

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        Skills = new Skills(this);
    }
}
