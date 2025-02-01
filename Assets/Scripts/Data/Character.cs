using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    // 스킬
    public Skills Skills { get; set; }

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        Skills = new Skills(this);
    }
}
