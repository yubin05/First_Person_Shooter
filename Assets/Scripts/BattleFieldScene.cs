using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldScene : LocalSingleton<BattleFieldScene>
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SpawnPlayer();
        SpawnEnemy();
    }

    private void SpawnPlayer()
    {
        int playerId = 20001;
        var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), 110001);

        var pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
        var rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));
        
        var playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(playerId, pos, rot);
        GameApplication.Instance.GameController.GunController.Spawn<GunInfo, GunObject>(90001, playerObj);
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
}
