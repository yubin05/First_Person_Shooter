using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject
{
    [SerializeField] private HitEffecter hitEffecter;
    public HitEffecter HitEffecter => hitEffecter;

    public override void Init(Data data)
    {
        base.Init(data);

        var player = data as Player;
        player.Init(this);
    }
}
