using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP 시스템 (Ex: 캐릭터 오브젝트의 HP 등)
/// </summary>
public class HealthSystem : MonoBehaviour
{
    private float curHp;
    public float CurHp
    {
        get { return curHp; }
        set
        {
            curHp = value;
            ChangeCurHpEvent?.Invoke(curHp);
        }
    }

    private float maxHp;
    public float MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
            ChangeMaxHpEvent?.Invoke(maxHp);
        }
    }

    public event Action<float> ChangeCurHpEvent;    // 현재 체력이 변화할 때마다 호출되는 이벤트
    public event Action<float> ChangeMaxHpEvent;    // 최대 체력이 변화할 때마다 호출되는 이벤트
    
    public void Init(float maxHp)
    {
        MaxHp = maxHp;
        CurHp = maxHp;

        ChangeCurHpEvent = null;
        ChangeMaxHpEvent = null;
    }
}
