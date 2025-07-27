using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

public class GameManager : GlobalSingleton<GameManager>
{
    // 게임 시작 시, 자동으로 생성
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        instance = Instance;
        Debug.Log(Application.persistentDataPath);

        GameApplication.Instance.Init();
        Pooling.Instance.Init();
    }

    // private void Update()
    // {
    //     // if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F4))
    //     // {
    //     //     QuitGame();
    //     // }

    //     // // test
    //     // if (Input.GetKeyDown(KeyCode.Alpha1))
    //     // {
    //     //     GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10002, Vector3.zero, Quaternion.identity);
    //     // }
    //     // if (Input.GetKeyDown(KeyCode.Alpha2))
    //     // {
    //     //     var soundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo));
    //     //     if (soundInfos != null) Debug.Log(soundInfos.Length);
    //     // }
    // }

    // 언어 변경
    public void ChanageLanauage(TextInfo.LanguageTypes language)
    {
        GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language = language;

        var fileName = GameApplication.Instance.GameModel.ClientData.PlayerLanguageFileName;
        var extension = GameApplication.Instance.GameModel.JsonTable.Extension;

        var path = Application.persistentDataPath + "/" + DataTablePath.JsonFilePath + fileName + extension;
        var dataStr = "[" + JsonConvert.SerializeObject(GameApplication.Instance.GameModel.ClientData.PlayerLanguage) + "]";
        File.WriteAllText(path, dataStr);
    }

    // BGM 데이터 변경
    public void ChangeBGMVolume(float volume)
    {
        // 현재 재생되고 있는 BGM 볼륨 변경
        var bgmSoundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo)).Where(x => x.Type == SoundInfo.Types.BGM);
        foreach (var bgmSoundInfo in bgmSoundInfos)
        {
            var bgmSoundObj = bgmSoundInfo.MyObject as SoundObject;
            bgmSoundObj.AudioSource.volume = volume;
        }

        // BGM 볼륨 저장 데이터(클라이언트) 변경
        GameApplication.Instance.GameModel.ClientData.PlayerSound.bgm = volume;
        SavePlayerSound();
    }
    // SFX 데이터 변경
    public void ChangeSFXVolume(float volume)
    {
        // 현재 재생되고 있는 SFX 볼륨 변경
        var sfxSoundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo)).Where(x => x.Type == SoundInfo.Types.SFX);
        foreach (var sfxSoundInfo in sfxSoundInfos)
        {
            var sfxSoundObj = sfxSoundInfo.MyObject as SoundObject;
            sfxSoundObj.AudioSource.volume = volume;
        }

        // BGM 볼륨 저장 데이터(클라이언트) 변경
        GameApplication.Instance.GameModel.ClientData.PlayerSound.sfx = volume;
        SavePlayerSound();
    }
    // 사운드 데이터(클라이언트) 저장
    private void SavePlayerSound()
    {
        var fileName = GameApplication.Instance.GameModel.ClientData.PlayerSoundFileName;
        var extension = GameApplication.Instance.GameModel.JsonTable.Extension;

        var path = Application.persistentDataPath + "/" + DataTablePath.JsonFilePath + fileName + extension;
        var dataStr = "[" + JsonConvert.SerializeObject(GameApplication.Instance.GameModel.ClientData.PlayerSound) + "]";
        File.WriteAllText(path, dataStr);
    }

    private float cacheTime = 1.0f;
    // 일시 정지 해제
    public void PlayGame()
    {
        Time.timeScale = cacheTime;
    }
    // 일시 정지
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // 게임 종료
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
