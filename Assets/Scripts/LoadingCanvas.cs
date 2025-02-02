using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvas : LocalSingleton<LoadingCanvas>
{
    [SerializeField] private LoadingPanel loadingPanel;
    public LoadingPanel LoadingPanel => loadingPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadingPanel.Init();
    }
}
