using UnityEngine.SceneManagement;

// 씬 변환할 때 사용하는 이벤트 정의하는 인터페이스
public interface IChangeScene
{
    public void OnChangeScene(Scene beforeScene, Scene afterScene);
}
