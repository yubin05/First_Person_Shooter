using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 데이터 + 게임 컨트톨러 등을 관리할 클래스
public class GameApplication : GlobalSingleton<GameApplication>, IChangeScene
{
    public GameModel GameModel { get; } = new GameModel();
    public GameController GameController { get; } = new GameController();

    public void Init()
    {
        GameModel.Init();
        GameController.Init(GameModel);

        SceneManager.activeSceneChanged -= OnChangeScene;
        SceneManager.activeSceneChanged += OnChangeScene;
    }
    public void OnChangeScene(Scene beforeScene, Scene afterScene)
    {
        GameModel.RuntimeData.Clear();
    }
}
