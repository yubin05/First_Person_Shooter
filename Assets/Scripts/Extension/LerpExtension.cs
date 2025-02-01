using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector Lerp 확장 메서드
/// </summary>
public static class LerpExtension
{
    /// <summary>
    /// 시간(t)에 따른 베지에 곡선 Vector3 위치(f)를 반환하는 함수
    /// </summary>
    /// <param name="a">출발 지점</param>
    /// <param name="b">중간 지점->해당 위치에 따라 곡선의 모양이 결정 됨</param>
    /// <param name="c">도착 지점</param>
    /// <param name="t">시간(0~1)</param>
    /// <returns></returns>
    public static Vector3 BezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 d = Vector3.Lerp(a, b, t);  // a와 b의 선형 보간
        Vector3 e = Vector3.Lerp(b, c, t);  // b와 c의 선형 보간
        Vector3 f = Vector3.Lerp(d, e, t);  // d와 e의 선형 보간 -> 최종적으로 곡선을 그리는 벡터
        return f;
    }
}
