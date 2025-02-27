using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 모든 게임 컨트롤러를 모아놓은 클래스
/// </summary>
public class GameController
{
    public SoundController SoundController { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public EnemyController EnemyController { get; private set; }
    public GunController GunController { get; private set; }
    public KnifeController KnifeController { get; private set; }
    public BulletController BulletController { get; private set; }    

    public void Init(GameModel gameModel)
    {
        SoundController = new SoundController(gameModel);
        PlayerController = new PlayerController(gameModel);
        EnemyController = new EnemyController(gameModel);
        GunController = new GunController(gameModel);
        KnifeController = new KnifeController(gameModel);
        BulletController = new BulletController(gameModel);        
    }
}

/// <summary>
/// 모든 컨트롤러의 베이스(부모)가 되는 클래스
/// </summary>
public abstract class BaseController
{
    protected GameModel GameModel;

    public BaseController(GameModel gameModel)
    {
        GameModel = gameModel;
    }

    public virtual K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent=null) where T : Data where K : PoolObject
    {
        T data = GameModel.PresetData.ReturnData<T>(typeof(T).Name, id).Clone() as T;
        GameModel.RuntimeData.AddData(typeof(T).Name, data);    // 데이터 추가

        K obj = Pooling.Instance.CreatePoolObject<K>(id);   // 오브젝트 풀링 가져오기
        obj.transform.parent = parent;
        if (parent == null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        else
        {
            obj.transform.localPosition = position;
            obj.transform.localRotation = rotation;
        }
        obj.Init(data);

        data.OnDataRemove = null;
        data.OnDataRemove += (_data) => 
        {
            GameModel.RuntimeData.RemoveData(typeof(T).Name, _data);    // 데이터 삭제
            Pooling.Instance.ReturnPoolObject(id, obj); // 오브젝트 풀링 반납
        };
        
        return obj;
    }
}

/// <summary>
/// 사운드 관련 컨트롤러 (사운드 오브젝트 소환 등)
/// </summary>
public class SoundController : BaseController
{
    public SoundController(GameModel gameModel) : base(gameModel)
    {
    }

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var soundObj = base.Spawn<T, K>(id, position, rotation, parent) as SoundObject;
        var sound = soundObj.data as SoundInfo;

        switch (sound.Type)
        {
            case SoundInfo.Types.BGM: default: soundObj.AudioSource.volume = GameModel.ClientData.PlayerSound.bgm; break;
            case SoundInfo.Types.SFX: soundObj.AudioSource.volume = GameModel.ClientData.PlayerSound.sfx; break;
        }

        return soundObj as K;
    }

    public void ChangeBGMVolume(float volume) => GameManager.Instance.ChangeBGMVolume(volume);
    public void ChangeSFXVolume(float volume) => GameManager.Instance.ChangeSFXVolume(volume);
}

public class CharacterController2 : BaseController
{
    public CharacterController2(GameModel gameModel) : base(gameModel) {}

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var characterObj = base.Spawn<T, K>(id, position, rotation, parent) as CharacterObject;
        var character = characterObj.data as Character;

        // 캐릭터 데이터가 삭제되면 해당 캐릭터가 갖고 있던 총 데이터도 삭제
        character.OnDataRemove += (_data) => 
        {
            if (characterObj.WeaponObject != null) characterObj.WeaponObject.data.RemoveData();
        };

        var actorStatInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<ActorStatInfo>(nameof(ActorStatInfo), id);
        character.BasicActorStat = actorStatInfo;

        if (characterObj.TryGetComponent<HealthSystem>(out var healthSystem))
        {
            healthSystem.Init(actorStatInfo.MaxHp);

            healthSystem.ChangeCurHpEvent += (hp) => 
            {
                if (hp <= 0) characterObj.OnDeath();    // 체력이 0이하이면 죽은 것으로 판정
            };
        }

        return characterObj as K;
    }
}
public class PlayerController : CharacterController2
{
    public PlayerController(GameModel gameModel) : base(gameModel) {}

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var playerObj = base.Spawn<T, K>(id, position, rotation, parent) as PlayerObject;
        // var player = playerObj.data as Player;

        // 총 장착 - 총 아이디 임시 하드 코딩
        // playerObj.OnTake<GunInfo, RifleObject>(90001);
        playerObj.OnTake<GunInfo, PistolObject>(90002);

        return playerObj as K;
    }
}
public class EnemyController : CharacterController2
{
    public EnemyController(GameModel gameModel) : base(gameModel) {}

    public K Spawn<T, K>(int id, int spawnPointId, Transform parent=null) where T : Enemy where K : EnemyObject
    {
        var spawnPointInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<SpawnPointInfo>(nameof(SpawnPointInfo), spawnPointId);

        var pos = new Vector3(spawnPointInfo.PositionX, spawnPointInfo.PositionY, spawnPointInfo.PositionZ);
        var rot = Quaternion.Euler(new Vector3(spawnPointInfo.RotationX, spawnPointInfo.RotationY, spawnPointInfo.RotationZ));

        var enemyObj = base.Spawn<T, K>(id, pos, rot, parent);
        var enemy = enemyObj.data as T;

        enemy.OnDataRemove += (data) => 
        {
            if (BattleFieldScene.Instance != null) BattleFieldScene.Instance.StartCoroutine(BattleFieldScene.Instance.RespawnEnemy(id, spawnPointId, enemy.RespawnTime));
        };

        return enemyObj;
    }
}

public abstract class WeaponController : BaseController
{
    public WeaponController(GameModel gameModel) : base(gameModel) {} 

    public virtual K Spawn<T, K>(int id, CharacterObject characterObj) where T : WeaponInfo where K : WeaponObject
    {
        var weaponObj = Spawn<T, K>(id, Vector3.zero, Quaternion.identity, characterObj.WeaponNode);
        var weapon = weaponObj.data as T;

        // 무기 데이터가 사라지면 무기 장착하고 있는 플레이어의 무기 오브젝트 데이터 삭제
        // 무기 데이터가 사라지면 무기 오브젝트 데이터의 소유자 오브젝트 데이터 삭제
        weapon.OnDataRemove += (data) => 
        {
            characterObj.WeaponObject = null;
            weaponObj.OwnerObject = null;
        };

        weaponObj.OwnerObject = characterObj;
        characterObj.WeaponObject = weaponObj;
        return weaponObj;
    }
}
public class GunController : WeaponController
{
    public GunController(GameModel gameModel) : base(gameModel) {}

    public override K Spawn<T, K>(int id, CharacterObject characterObj)
    {
        var gunObj = base.Spawn<T, K>(id, characterObj) as GunObject;
        var gun = gunObj.data as GunInfo;

        var character = characterObj.data as Character;
        gun.AimingTime = character.BasicActorStat.AimingTime;
        gun.NoZoomFieldOfView = gunObj.OwnerObject.Cam.fieldOfView;
        gun.ZoomFieldOfView = gun.NoZoomFieldOfView / gun.AimRate;

        // 격발 시, 반동 시스템의 반동 기능 작동
        var reactionSystem = characterObj.Cam.transform.GetComponent<ReactionSystem>();
        if (reactionSystem != null)
        {
            gunObj.ShotEvent += () => 
            {
                reactionSystem.React();
            };
        }
        
        return gunObj as K;
    }
}
public class KnifeController : WeaponController
{
    public KnifeController(GameModel gameModel) : base(gameModel) {}

    public override K Spawn<T, K>(int id, CharacterObject characterObj)
    {
        var knifeObj = base.Spawn<T, K>(id, characterObj) as KnifeObject;
        var knife = knifeObj.data as KnifeInfo;

        knifeObj.HitEvent += () => 
        {
            if (knifeObj.OwnerObject != null) knifeObj.OwnerObject.HandsObjectSystem.CurHandsObject.WeaponObject.WeaponHUD.BlinkHitCanvas(0.3f);
        };

        return knifeObj as K;
    }
}

public class BulletController : BaseController
{
    public BulletController(GameModel gameModel) : base(gameModel) {}

    public K Spawn<T, K>(int id, GunObject gunObject, Vector3 targetPos) where T : Bullet where K : BulletObject
    {
        var bulletObj = Spawn<T, K>(id, gunObject.MuzzleNode.position, Quaternion.identity);
        var bullet = bulletObj.data as T;

        bulletObj.transform.LookAt(targetPos);

        bulletObj.HitEvent += () => 
        {
            if (gunObject.OwnerObject != null) gunObject.OwnerObject.HandsObjectSystem.CurHandsObject.WeaponObject.WeaponHUD.BlinkHitCanvas(0.1f);
        };

        var gun = gunObject.data as GunInfo;
        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(gun.ShotSoundId, gunObject.OwnerObject.transform.position, Quaternion.identity);

        return bulletObj;
    }
}