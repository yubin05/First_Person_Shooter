using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    public enum Types { Five, Seven, Nine }
    public Types Type { get; set; }

    public float AttackPower { get; set; }
    public float MoveSpeed { get; set; }
}
