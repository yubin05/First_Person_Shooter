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
        GameApplication.Instance.GameController.GunController.Spawn<GunInfo, RifleObject>(90001, playerObj);
    }

    private void SpawnEnemy()
    {
        int botCount = Random.Range(1, 10);
        for (int i=0; i<botCount; i++)
        {
            int botId = 30001;
            int spawnPointId = Random.Range(110002, 110010);
            var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), spawnPointId);

            var pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
            var rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));

            var botObj = GameApplication.Instance.GameController.EnemyController.Spawn<Enemy, EnemyObject>(botId, pos, rot);
        }
    }

    private void OnDisable()
    {
        inputAction.BattleField_Setting.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<float>();
        if (inputValue == 0) IsPause = !IsPause;   // 0은 손에서 뗏을 때 (Up)
    }
}
