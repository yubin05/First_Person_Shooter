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
        int playerId = 20001;
        var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), 1);

        Vector3 pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
        Quaternion rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));
        
        var playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(playerId, pos, rot);
        GameApplication.Instance.GameController.GunController.Spawn<GunInfo, GunObject>(90001, playerObj);
    }
}
