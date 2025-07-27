using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleFieldScene_Pause : MonoBehaviour
{
    private bool isPause;
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause) OnPause();
            else OffPause();
        }
    }

    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI txt_pause;

    private void Start()
    {
        IsPause = false;
    }

    // 일시정지 되었을 때
    private void OnPause()
    {
        txt_pause.UpdateTextInfoName(1005);
        root.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    // 일시정지 해제 되었을 때
    private void OffPause()
    {
        root.SetActive(false);
        GameManager.Instance.PlayGame();
    }
}
