using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP 시스템 (Ex: 캐릭터 오브젝트의 HP 등)
/// </summary>
public class HealthSystem : MonoBehaviour
{
    public ObservableField<float> CurHp;
    public ObservableField<float> MaxHp;
    
    public void Init(float maxHp)
    {
        MaxHp = new ObservableField<float>(maxHp);
        CurHp = new ObservableField<float>(maxHp);
    }
}
