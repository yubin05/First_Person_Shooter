using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSystem : MonoBehaviour, PlayerInputAction.IBattleField_PlayerActions
{
    [SerializeField] private PlayerObject playerObj;

    private PlayerInputAction inputAction;
    
    // TODO: 추후 설정을 통해 해당 값을 변경할 수 있도록 함으로써 하드 코딩 해제해야 함
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
        var inputRotVector = inputAction.BattleField_Player.Look.ReadValue<Vector2>();
        inputRotVector.x *= xSensitivity; inputRotVector.y *= ySensitivity;
        playerObj.transform.Rotate(0, inputRotVector.x, 0);
        playerObj.Cam.transform.Rotate(-inputRotVector.y, 0, 0);
    }

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
