using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BattleFieldScene_Camera : MonoBehaviour
{
    private Transform target;
    public Transform Target
    { 
        get
        {
            return target;
        }
        set
        {
            target = value;
            gameObject.SetActive(target != null);
        }
    }

    private void Update()
    {
        if (Target != null)
        {
            transform.position = Target.position;
            transform.rotation = Target.rotation;
            transform.localScale = Target.localScale;
        }
    }
}
