using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 클래스를 상속받은 클래스의 Instance를 호출하면 자동으로 전역 싱글턴 패턴으로 게임 오브젝트 생성
// Ex) GameManager : GlobalSingleTon<GameManager>
public class GlobalSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject($"@{typeof(T).Name}").AddComponent<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}
