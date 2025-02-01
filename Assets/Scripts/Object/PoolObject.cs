using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public Data data { get; protected set; }
    public virtual void Init(Data data)
    {
        this.data = data;
    }
}
