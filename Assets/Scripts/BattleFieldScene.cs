using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleFieldScene : LocalSingleton<BattleFieldScene>, PlayerInputAction.IBattleField_SettingActions
{
    public bool IsPause { get; set; }   // 게임이 일시정지인지 체크
    private PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.BattleField_Setting.SetCallbacks(this);    // 콜백 함수 사용하기 위해 등록해야 함
        inputAction.BattleField_Setting.Enable();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SpawnPlayer();
        SpawnEnemy();
        IsPause = false;
    }

    private void SpawnPlayer()
    {
        int playerId = 20001;
        var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), 110001);

        var pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
        var rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));
        
        var playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(playerId, pos, rot);
    }

    private void SpawnEnemy()
    {
        var spawnPointInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<SpawnPointInfo>(nameof(SpawnPointInfo));
        for (int i=1; i<spawnPointInfos.Length; i++)
        {
            int botId = 30002;
            int spawnPointId = spawnPointInfos[i].Id;

            GameApplication.Instance.GameController.EnemyController.Spawn<Enemy, EnemyObject>(botId, spawnPointId);
        }
    }

    private void OnDisable()
    {
        inputAction.BattleField_Setting.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValueAsButton();
        if (inputValue == false) IsPause = !IsPause;   // false = 손에서 뗏을 때 (Up)
    }
}
