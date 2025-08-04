using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CharacterObject
{
    public EnemyObject_AI EnemyObject_AI { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        EnemyObject_AI = GetComponent<EnemyObject_AI>();
    }

    public override void Init(Data data)
    {
        base.Init(data);

        var enemy = data as Enemy;
        enemy.Init(this);

        EnemyObject_AI?.Init(this);
    }
}