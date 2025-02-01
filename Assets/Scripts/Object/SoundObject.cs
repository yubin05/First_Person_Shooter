using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 정보를 담은 게임 오브젝트
[RequireComponent(typeof(AudioSource))]
public class SoundObject : EntityObject
{
    public AudioSource AudioSource { get; protected set; }

    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public override void Init(Data data)
    {
        base.Init(data);
        
        var soundInfo = data as SoundInfo;
        
        if (soundInfo.Type == SoundInfo.Types.BGM)
        {
            AudioSource.loop = true;
        }
        else
        {
            AudioSource.loop = false;
            soundInfo.StartLifeTime(AudioSource.clip.length);
        }

        AudioSource.Play();
    }
}
