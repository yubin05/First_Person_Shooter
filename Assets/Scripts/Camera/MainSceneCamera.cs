using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCamera : MonoBehaviour
{
    private float startSpeed = 0.00005f;

    private void Update()
    {
        transform.Translate(Vector3.back*startSpeed, Space.World);
        transform.Translate(Vector3.up*startSpeed, Space.World);
    }
}
