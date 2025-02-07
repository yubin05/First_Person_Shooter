using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : EntityObject
{
    private void Update()
    {
        transform.Translate(Vector3.forward, Space.Self);
    }
}
