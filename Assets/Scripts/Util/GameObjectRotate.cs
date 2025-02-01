using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRotate : MonoBehaviour
{
    [SerializeField] private float rotateX;
    [SerializeField] private float rotateY;
    [SerializeField] private float rotateZ;

    private void Update()
    {
        var rotation = new Vector3(rotateX, rotateY, rotateZ);
        transform.Rotate(rotation);
    }
}
