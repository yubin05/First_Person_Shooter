using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour, PlayerInputAction.IBattleField_PlayerActions
{
    [SerializeField] private PlayerObject playerObj;
    [SerializeField] private Transform upperBodyTrans;

    private PlayerInputAction inputAction;
    
    // temp
    private Vector3 inputRotVector;
    private float xSensitivity = 0.1f;
    private float ySensitivity = 0.1f;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.BattleField_Player.SetCallbacks(this);
        inputAction.BattleField_Player.Enable();
    }

    private void Update()
    {
        if (BattleFieldScene.Instance.IsPause) return;

        var player = playerObj.data as Player;

        // 이동
        var inputMoveVector = inputAction.BattleField_Player.Move.ReadValue<Vector2>();
        var moveVector = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
        if (moveVector != Vector3.zero) playerObj.OnMove(moveVector * player.BasicActorStat.MoveSpeed * Time.deltaTime);
        else playerObj.OnIdle();

        // 회전
        inputRotVector = inputAction.BattleField_Player.Look.ReadValue<Vector2>();
        inputRotVector.x *= xSensitivity; inputRotVector.y *= ySensitivity;
        playerObj.transform.Rotate(0, inputRotVector.x, 0);
        playerObj.Cam.transform.RotateAround(upperBodyTrans.transform.position, playerObj.transform.right, -inputRotVector.y);        
    }
    // private void LateUpdate()
    // {
    //     // 회전 - 애니메이터로 인해 Update문에서는 회전이 Block되는 문제 있음
    //     upperBodyTrans.transform.RotateAround(upperBodyTrans.position, playerObj.transform.right, -inputRotVector.y);
    // }

    // private void OnAnimatorMove()
    // {
    //     // 회전 - 애니메이터로 인해 Update문에서는 회전이 Block되는 문제 있음
    //     upperBodyTrans.transform.Rotate(0, 0, -inputRotVector.y);
    // }

    private void OnDisable()
    {
        inputAction.BattleField_Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }
    public void OnLook(InputAction.CallbackContext context)
    {
    }
}
