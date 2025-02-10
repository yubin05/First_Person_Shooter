using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour, PlayerInputAction.IBattleField_PlayerActions
{
    [SerializeField] private PlayerObject playerObj;
    [SerializeField] private Camera playerCam;

    private PlayerInputAction inputAction;

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
        playerObj.OnMove(moveVector * player.BasicActorStat.MoveSpeed * Time.deltaTime);

        // 회전
        var inputRotVector = inputAction.BattleField_Player.Look.ReadValue<Vector2>();
        var rotVector = new Vector2(-(inputRotVector.y*0.1f), inputRotVector.x*0.1f);
        playerObj.transform.Rotate(0, rotVector.y, 0);
        playerCam.transform.Rotate(rotVector.x, 0, 0);
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
