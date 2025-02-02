using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 모든 게임 컨트롤러를 모아놓은 클래스
/// </summary>
public class GameController
{
    public SoundController SoundController { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public GunController GunController { get; private set; }

    public void Init(GameModel gameModel)
    {
        SoundController = new SoundController(gameModel);
        PlayerController = new PlayerController(gameModel);
        GunController = new GunController(gameModel);
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
        var soundObj = base.Spawn<T, K>(id, position, rotation, parent);
        var sound = soundObj.data as SoundInfo;

        switch (sound.Type)
        {
            case SoundInfo.Types.BGM: default: (soundObj as SoundObject).AudioSource.volume = GameModel.ClientData.PlayerSound.bgm; break;
            case SoundInfo.Types.SFX: (soundObj as SoundObject).AudioSource.volume = GameModel.ClientData.PlayerSound.sfx; break;
        }

        return soundObj;
    }

    public void ChangeBGMVolume(float volume) => GameManager.Instance.ChangeBGMVolume(volume);
    public void ChangeSFXVolume(float volume) => GameManager.Instance.ChangeSFXVolume(volume);
}

public class PlayerController : BaseController
{
    public PlayerController(GameModel gameModel) : base(gameModel) {}
}

public class GunController : BaseController
{
    public GunController(GameModel gameModel) : base(gameModel) {}

    public K Spawn<T, K>(int id, CharacterObject characterObj) where T : GunInfo where K : GunObject
    {
        var gunObj = Spawn<T, K>(id, Vector3.zero, Quaternion.identity, characterObj.GunNode);
        var gun = gunObj.data as T;

        // 총 데이터가 사라지면 해당 총을 장착하고 있는 플레이어의 총 오브젝트 데이터 삭제
        gun.OnDataRemove += (data) => 
        {
            characterObj.GunObject = null;
        };

        characterObj.GunNode.transform.localPosition = gun.GetNoAimPos();
        characterObj.GunNode.transform.localRotation = gun.GetNoAimRot();
        characterObj.GunNode.transform.localScale = Vector3.one * gun.NoAimScale;
        characterObj.GunObject = gunObj;
        return gunObj;
    }
}