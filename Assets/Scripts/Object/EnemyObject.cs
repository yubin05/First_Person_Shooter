using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CharacterObject
{
    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();    // temp
    }

    public override void Init(Data data)
    {
        base.Init(data);

        var enemy = data as Enemy;
        enemy.Init(this);
    }
}
