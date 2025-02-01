using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerExtension
{
    // 해당 트랜스폼 포함 하위 객체까지 모든 레이어를 변경해주는 함수
    public static void ChangeLayerRecursively(this Transform target, string layerName)
    {
        target.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in target)
        {
            child.ChangeLayerRecursively(layerName);
        }
    }
}
