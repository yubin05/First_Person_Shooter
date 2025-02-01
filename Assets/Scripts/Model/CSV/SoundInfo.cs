using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 정보를 담은 클래스
public class SoundInfo : Entity
{
    public enum Types { BGM, SFX }
    public Types Type { get; set; }
}
