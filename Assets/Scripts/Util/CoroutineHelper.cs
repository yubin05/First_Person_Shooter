using System.Collections;
using UnityEngine;

// 코루틴 관리하기 위한 클래스
// Ex) 스킬 코루틴만 모두 정지하고 싶으면, 해당 클래스로 Start 후, StopAll하면 됨
public class CoroutineHelper : GlobalSingleton<CoroutineHelper>
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = Instance;
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }

    public new static void StopAllCoroutines()
    {
        monoInstance.StopAllCoroutines();
    }
}
