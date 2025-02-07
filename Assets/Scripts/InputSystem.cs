using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    // Show In Inspector
    [SerializeField] private PlayerObject playerObj;
    [SerializeField] private Camera playerCam;

    // Hide In Inspector;
    private Vector3 prevPos;

    // start
    private void Start()
    {
        prevPos = Input.mousePosition;
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        if (!GameManager.Instance.IsPause)
        {
            playerObj.transform.Rotate(0, (mousePos.x-prevPos.x)*0.1f, 0);
            playerCam.transform.Rotate((prevPos.y-mousePos.y)*0.1f, 0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                playerObj.OnMove(Vector3.forward * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerObj.OnMove(Vector3.back * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                playerObj.OnMove(Vector3.left * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                playerObj.OnMove(Vector3.right * Time.deltaTime);
            }

            // 조준
            if (Input.GetMouseButton(1))
            {
                playerObj.OnAiming(true, 0.5f);
            }
            else
            {
                playerObj.OnAiming(false, 0.5f);
            }

            // 공격
            if (Input.GetMouseButtonDown(0))
            {
                playerObj.OnAttack();
            }
        }
        
        prevPos = mousePos;        
    }
}
