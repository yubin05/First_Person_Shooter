using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class BattleFieldScene_Player_Death_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Info;
    [SerializeField] private TextMeshProUGUI txt_RespawnTime;

    public bool IsProcessing { get; private set; } = false;
    public Action OnReviveProcessEnd = null;

    private float time;
    public float Time
    {
        get
        {
            return time;
        }
        set
        {
            // 타이머가 작동하는 동안은 중간에 업데이트 불가
            if (IsProcessing) return;
            IsProcessing = true;

            time = value;
            gameObject.SetActive(true);
            ReviveProcess(time).Forget();
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // private void Init(float reviveTime) => ReviveProcess(reviveTime).Forget();
    private async UniTask ReviveProcess(float reviveTime)
    {
        time = reviveTime;
        txt_Info.UpdateTextInfoName(1006);
        txt_RespawnTime.text = $"{time:00}";

        while (time > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            time--;
            txt_RespawnTime.text = $"{time:00}";
        }

        IsProcessing = false;
        gameObject.SetActive(false);
        OnReviveProcessEnd?.Invoke();
        OnReviveProcessEnd = null;
    }
}
