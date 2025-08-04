using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleFieldScene : LocalSingleton<BattleFieldScene>, PlayerInputAction.IBattleField_SettingActions
{
    [SerializeField] private BattleFieldScene_Pause battleFieldScene_Pause;
    public bool IsPause { get => battleFieldScene_Pause.IsPause; set => battleFieldScene_Pause.IsPause = value; }   // 게임이 일시정지인지 체크

    [SerializeField] private BattleFieldScene_Camera battleFieldScene_Camera;
    public BattleFieldScene_Camera BattleFieldScene_Camera => battleFieldScene_Camera;

    [SerializeField] private BattleFieldScene_Player_Death_UI battleFieldScene_Player_Death_UI;
    public BattleFieldScene_Player_Death_UI BattleFieldScene_Player_Death_UI => battleFieldScene_Player_Death_UI;

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

    public void SpawnPlayer(int playerId=Define.DEFAULT_PLAYER_ID)
    {
        var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), 110001);

        var pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
        var rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));

        var playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(playerId, pos, rot);
    }

    public void SpawnEnemy()
    {
        var spawnPointInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<SpawnPointInfo>(nameof(SpawnPointInfo));
        for (int i = 1; i < spawnPointInfos.Length; i++)
        {
            int botId = Define.DEFAULT_ENEMY_ID;
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
        if (inputValue == false)
        {
            IsPause = !IsPause;   // false = 손에서 뗏을 때 (Up)
        }
    }
}